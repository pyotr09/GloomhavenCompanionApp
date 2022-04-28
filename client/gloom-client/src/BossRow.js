import React, {useState} from "react";
import {Box, IconButton, Table, TableBody, TableCell, TableHead, TableRow} from "@mui/material";
import LoopIcon from "@mui/icons-material/Loop";
import MonsterAdder from "./MonsterAdder";
import Button from "@mui/material/Button";
import MonsterRow from "./MonsterRow";
import RemoveIcon from "@mui/icons-material/Remove";
import AddIcon from "@mui/icons-material/Add";
import DeleteIcon from "@mui/icons-material/Delete";
import ShieldOutlinedIcon from "@mui/icons-material/ShieldOutlined";
import PanToolOutlinedIcon from "@mui/icons-material/PanToolOutlined";
import ArrowCircleRightOutlinedIcon from "@mui/icons-material/ArrowCircleRightOutlined";

export default function(props) {
    const [currHP, setHP] = useState(props.boss.MaxHealth);
    return (
        <React.Fragment>
            <TableRow>
                <TableCell>{getInitiative()}</TableCell>
                <TableCell>{props.boss.Name}</TableCell>
                <TableCell>{getActions()}<LoopIcon visibility={isShuffle() ? 'visible' : 'hidden'}/></TableCell>
                <TableCell><Button disabled={props.boss.IsActive} onClick={handleAdd}>Add Boss</Button></TableCell>
                <TableCell />
            </TableRow>
            {props.boss.IsActive ? <TableRow>
                <TableCell style={{paddingBottom: 0, paddingTop: 0}} colSpan={6}>
                    <Box sx={{margin: 1}}>
                        <Table size="small" aria-label="purchases">
                            <TableBody>
                                <TableRow>
                                    <TableCell>
                                        {getDefenses()}
                                    </TableCell>
                                    <TableCell>
                                        <IconButton disabled={currHP === 0} onClick={() => setHP(currHP - 1)}>
                                            <RemoveIcon />
                                        </IconButton>
                                        <IconButton disabled={currHP === props.boss.MaxHealth} onClick={() => setHP(currHP + 1)}>
                                            <AddIcon />
                                        </IconButton>
                                        {currHP}
                                        /{props.boss.MaxHealth}</TableCell>
                                    <TableCell>
                                        <IconButton>
                                            <DeleteIcon />
                                        </IconButton>
                                    </TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    </Box>
                </TableCell>
            </TableRow> : ""}
        </React.Fragment>
    );

    function getDefenses() {
        return <div>
            <div style={{ display: (props.boss.BaseShield > 0 ? 'block' : 'none') }}>
                <ShieldOutlinedIcon /> {props.boss.BaseShield}
            </div>
            <div style={{ display: (props.boss.BaseRetaliate > 0 ? 'block' : 'none') }}>
                <PanToolOutlinedIcon /> {props.boss.BaseRetaliate}
                <div style={{ display: (props.boss.BaseRetaliateRange > 1 ? 'block' : 'none') }}>
                    <ArrowCircleRightOutlinedIcon /> {props.boss.BaseRetaliateRange}
                </div>
            </div>
            <div style={{ display: (props.boss.DoAttackersGainDisadvantage ? 'block' : 'none') }}>
                Attackers gain Disadvantage
            </div>
        </div>
    }

    function getInitiative() {
        if (props.boss.Initiative === null)
            return "";
        else return parseInt(props.boss.Initiative);
    }

    function getActions() {
        if (props.boss.ActiveAbilityCard === null)
            return "";
        return props.boss.ActiveAbilityCard.Actions.map(a => a.EliteActionText).join(", ");
    }

    function isShuffle() {
        if (props.boss.ActiveAbilityCard === null)
            return false;
        return props.boss.ActiveAbilityCard.ShuffleAfter;
    }

    function handleAdd()  {
        const body = JSON.stringify(
            {
                "PreviousState": JSON.stringify(props.scenario),
                "GroupName": props.boss.Name
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
            `http://127.0.0.1:3000/drawforgroup`,
            init)
            .then(r => r.json())
            .then(json => {
                props.setScenarioState(json);
            });
    }
}