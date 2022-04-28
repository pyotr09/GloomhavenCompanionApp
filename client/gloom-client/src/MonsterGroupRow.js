import {Box, Table, TableBody, TableCell, TableHead, TableRow, Typography} from "@mui/material";
import React, {useState} from "react";
import MonsterAdder from "./MonsterAdder";
import MonsterRow from "./MonsterRow";
import LoopIcon from "@mui/icons-material/Loop";

export default function(props) {
    const [loading, setLoading] = useState(false);
    const standeeNumbers = Array.from({length: props.row.Count}, (_, i) => i+1);

    return (<React.Fragment>
        <TableRow>
            <TableCell>{getInitiative()}</TableCell>
            <TableCell>{props.row.Name}</TableCell>
            <TableCell>{getActions()}<LoopIcon visibility={isShuffle() ? 'visible' : 'hidden'}/></TableCell>
            <TableCell><MonsterAdder availableNums={standeeNumbers} tier="elite" add={addMonster} group={props.row} /></TableCell>
            <TableCell><MonsterAdder availableNums={standeeNumbers} tier="normal" add={addMonster} group={props.row}  /></TableCell>
        </TableRow>
        <TableRow>
            <TableCell style={{paddingBottom: 0, paddingTop: 0}} colSpan={6}>
                <Box sx={{margin: 1}}>
                    <Table size="small">
                        <TableHead>
                            <TableRow>
                                <TableCell>Tier</TableCell>
                                <TableCell>Number</TableCell>
                                <TableCell>Defense</TableCell>
                                <TableCell>HP</TableCell>
                                <TableCell>Abilities</TableCell>
                                <TableCell />
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {props.row.Monsters.sort(sortMonsters).map((monster) => (
                                <MonsterRow
                                    key={monster.MonsterNumber}  
                                    getTierText={getTierText}
                                    getActions={getActions}
                                    monster={monster}
                                    shuffle={isShuffle()}
                                />
                            ))}
                        </TableBody>
                    </Table>
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

    function finished() {
        setLoading(false);
    }

    function makeMonsterAPICall(tier, name, num) {
        const body = JSON.stringify(
            {
                "Level": props.scenario.Level.toString(),
                "Name": name,
                "Tier": tier,
                "Number": num.toString()
            }
        );
        console.log(body);
        let init = {
            method: 'POST',
            headers: {
                'Content-Type': 'text/plain'
            },
            body: body

        };
        console.log(init);
        fetch(
            `http://127.0.0.1:3000/addmonster`,
            init)
            .then(r => r.json())
            .then(json => {
                const ogCount = props.row.Monsters.length;
                props.addMonster(props.row.Name, json);
                if (ogCount === 0 && !props.scenario.IsBetweenRounds) {
                    drawOnlyForGroup();
                }
                finished();
            });
    }

    function drawOnlyForGroup() {
        const body = JSON.stringify(
            {
                "PreviousState": JSON.stringify(props.scenario),
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
            `http://127.0.0.1:3000/drawforgroup`,
            init)
            .then(r => r.json())
            .then(json => {
                props.setScenarioState(json);
            });
    }
}