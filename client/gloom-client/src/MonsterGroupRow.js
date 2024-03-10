import {Box, Icon, Table, TableBody, TableCell, TableHead, TableRow, Typography} from "@mui/material";
import React, {useState} from "react";
import MonsterAdder from "./MonsterAdder";
import MonsterRow from "./MonsterRow";
import ShuffleImage from "./images/Shuffle.svg";
import {apiUrl} from "./Constants";

export default function(props) {
    const [loading, setLoading] = useState(false);
    const standeeNumbers = Array.from({length: props.row.count}, (_, i) => i+1);

    return (<React.Fragment>
        <TableRow>
            <TableCell>{getInitiative()}</TableCell>
            <TableCell>{props.row.name}</TableCell>
            <TableCell>{getActions()}                
                {isShuffle() ?
                <Icon>
                    <img src={ShuffleImage} alt={props.alt} style={{height: "100%"}} />
                </Icon>
                : ""
            }</TableCell>
            <TableCell><MonsterAdder availableNums={standeeNumbers} tier="elite" add={addMonster} group={props.row} /></TableCell>
            <TableCell><MonsterAdder availableNums={standeeNumbers} tier="normal" add={addMonster} group={props.row}  /></TableCell>
        </TableRow>
        <TableRow>
            <TableCell style={{paddingBottom: 0, paddingTop: 0}} colSpan={6}>
                <Box sx={{margin: 1}}>
                    {props.row.monsters.length > 0 ?
                    <Table size="small">
                        <TableHead>
                            <TableRow>
                                <TableCell />
                                <TableCell align={"center"}>Status</TableCell>
                                <TableCell>DEF</TableCell>
                                <TableCell>HP</TableCell>
                                <TableCell>Abilities</TableCell>
                                <TableCell />
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {props.row.monsters.sort(sortMonsters).map((monster) => (
                                <MonsterRow
                                    key={monster.monsterNumber}  
                                    getActions={getActions}
                                    monster={monster}
                                    shuffle={isShuffle()}
                                    removeMonster={removeMonster}
                                    groupName={props.row.name}
                                    setScenarioState={props.setScenarioState}
                                    sessionId={props.sessionId}
                                />
                            ))}
                        </TableBody>
                    </Table> : "" }
                </Box>
            </TableCell>
        </TableRow>
    </React.Fragment>);

    function sortMonsters(m1,m2) {
        if (m1.Tier > m2.Tier) {
            return 1;
        } else if (m1.Tier < m2.Tier) {
            return -1;
        }

        // Else go to the 2nd item
        if (m1.MonsterNumber < m2.MonsterNumber) {
            return -1;
        } else if (m1.MonsterNumber > m2.MonsterNumber) {
            return 1
        } else { 
            return 0;
        }
    }

    function getTierText(t) {
        if (t === 0)
            return "Boss";
        if (t === 1) {
            return "Named"
        }
        if (t === 2) {
            return "Elite";
        }
        if (t === 3) {
            return "Normal";
        }
    }

    function isShuffle() {
        if (props.row.monsters.length === 0)
            return false;
        if (props.row.activeAbilityCard === null)
            return false;
        return props.row.activeAbilityCard.shuffleAfter;
    }

    function getInitiative() {
        if (props.row.initiative === null)
            return "";
        else return parseInt(props.row.initiative);
    }

    function getBaseActions() {
        if (props.row.monsters.length === 0)
            return "";
        if (props.row.activeAbilityCard === null)
            return "Click Draw to reveal monster abilities.";
        console.log('props.row.activeAbilityCard', props.row.activeAbilityCard);
        return props.row.activeAbilityCard.actions.map(a => a.baseActionText).join(", ");
    }
    
    function getActions(tier) {
        if (tier === 2)
            return getEliteActions();
        if (tier === 3)
            return getNormalActions();
        return getBaseActions();
    }
    
    function getEliteActions() {
        if (props.row.activeAbilityCard === null)
            return "";
        return props.row.activeAbilityCard.actions.map(a => a.eliteActionText).join(", ");
    }

    function getNormalActions() {
        if (props.row.activeAbilityCard === null)
            return "";
        return props.row.activeAbilityCard.actions.map(a => a.normalActionText).join(", ");

    }

    function addMonster(tier, num) {
        setLoading(true);
        makeMonsterAPICall(tier, props.row.name, num);
    }
    
    function removeMonster(num) {
        const body = JSON.stringify(
            {
                "SessionId": props.sessionId.toString(),
                "GroupName": props.row.name,
                "Number": num.toString()
            });
        let init = {
            method: 'POST',
            headers: {
                'Content-Type': 'text/plain'
            },
            body: body
        };
        fetch(
            `${apiUrl}/removemonster`, init)
            .then(r => r.json())
            .then(json => {
                props.setScenarioState(json)
            });

    }

    function finished() {
        setLoading(false);
    }

    function makeMonsterAPICall(tier, name, num) {
        const body = JSON.stringify(
            {
                "GroupName": name,
                "Tier": tier,
                "Number": num.toString()
            }
        );
        let init = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: body

        };
        fetch(
            `${apiUrl}/${props.sessionId}/addmonster`,
            init)
            .then(r => r.json())
            .then(json => {
                const ogCount = props.row.monsters.length;
                props.setScenarioState(json);
                if (ogCount === 0 && !props.scenario.isBetweenRounds) {
                    drawOnlyForGroup();
                }
                finished();
            });
    }

    function drawOnlyForGroup() {
        const body = JSON.stringify(
            {
                "GroupName": props.row.name
            }
        );

        let init = {
            method: 'POST',
            headers: {
                'Content-Type': 'text/plain'
            },
            body: body

        };
        fetch(
            `${apiUrl}/${props.sessionId}/drawforgroup`,
            init)
            .then(r => r.json())
            .then(json => {
                props.setScenarioState(json);
            });
    }
}