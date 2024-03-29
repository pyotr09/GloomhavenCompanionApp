﻿import {Box, Icon, Table, TableBody, TableCell, TableHead, TableRow, Typography} from "@mui/material";
import React, {useState} from "react";
import MonsterAdder from "./MonsterAdder";
import MonsterRow from "./MonsterRow";
import ShuffleImage from "./images/Shuffle.svg";
import {apiUrl} from "./Constants";

export default function(props) {
    const [loading, setLoading] = useState(false);
    const standeeNumbers = Array.from({length: props.row.Count}, (_, i) => i+1);

    return (<React.Fragment>
        <TableRow>
            <TableCell>{getInitiative()}</TableCell>
            <TableCell>{props.row.Name}</TableCell>
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
                    {props.row.Monsters.length > 0 ?
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
                            {props.row.Monsters.sort(sortMonsters).map((monster) => (
                                <MonsterRow
                                    key={monster.MonsterNumber}  
                                    getActions={getActions}
                                    monster={monster}
                                    shuffle={isShuffle()}
                                    removeMonster={removeMonster}
                                    groupName={props.row.Name}
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
        if (props.row.Monsters.length === 0)
            return false;
        if (props.row.ActiveAbilityCard === null)
            return false;
        return props.row.ActiveAbilityCard.ShuffleAfter;
    }

    function getInitiative() {
        if (props.row.Initiative === null)
            return "";
        else return parseInt(props.row.Initiative);
    }

    function getBaseActions() {
        if (props.row.Monsters.length === 0)
            return "";
        if (props.row.ActiveAbilityCard === null)
            return "Click Draw to reveal monster abilities.";
        return props.row.ActiveAbilityCard.Actions.map(a => a.BaseActionText).join(", ");
    }
    
    function getActions(tier) {
        if (tier === 2)
            return getEliteActions();
        if (tier === 3)
            return getNormalActions();
        return getBaseActions();
    }
    
    function getEliteActions() {
        if (props.row.ActiveAbilityCard === null)
            return "";
        return props.row.ActiveAbilityCard.Actions.map(a => a.EliteActionText).join(", ");
    }

    function getNormalActions() {
        if (props.row.ActiveAbilityCard === null)
            return "";
        return props.row.ActiveAbilityCard.Actions.map(a => a.NormalActionText).join(", ");

    }

    function addMonster(tier, num) {
        setLoading(true);
        makeMonsterAPICall(tier, props.row.Name, num);
    }
    
    function removeMonster(num) {
        const body = JSON.stringify(
            {
                "SessionId": props.sessionId.toString(),
                "GroupName": props.row.Name,
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
                "SessionId": props.sessionId.toString(),
                "Name": name,
                "Tier": tier,
                "Number": num.toString()
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
            `${apiUrl}/addmonster`,
            init)
            .then(r => r.json())
            .then(json => {
                const ogCount = props.row.Monsters.length;
                props.setScenarioState(json);
                if (ogCount === 0 && !props.scenario.IsBetweenRounds) {
                    drawOnlyForGroup();
                }
                finished();
            });
    }

    function drawOnlyForGroup() {
        const body = JSON.stringify(
            {
                "SessionId": props.sessionId.toString(),
                "GroupName": props.row.Name
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
            `${apiUrl}/drawforgroup`,
            init)
            .then(r => r.json())
            .then(json => {
                props.setScenarioState(json);
            });
    }
}