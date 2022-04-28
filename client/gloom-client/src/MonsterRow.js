import {IconButton, TableCell, TableRow} from "@mui/material";
import React, {useState} from "react";
import LoopIcon from "@mui/icons-material/Loop";
import AddIcon from "@mui/icons-material/Add";
import RemoveIcon from "@mui/icons-material/Remove";
import ShieldOutlinedIcon from "@mui/icons-material/ShieldOutlined";
import PanToolOutlinedIcon from "@mui/icons-material/PanToolOutlined";
import DeleteIcon from "@mui/icons-material/Delete";
import ArrowCircleRightOutlinedIcon from "@mui/icons-material/ArrowCircleRightOutlined";

export default function(props) {
    const [currHP, setHP] = useState(props.monster.CurrentHitPoints);
    
    return(
        <TableRow>
            <TableCell component="th" scope="row">
                {props.getTierText(props.monster.Tier)}
            </TableCell>
            <TableCell>{props.monster.MonsterNumber}</TableCell>
            <TableCell>
                {getDefenses()}
            </TableCell>
            <TableCell>
                <IconButton disabled={currHP === 0} onClick={() => setHP(currHP - 1)}>
                    <RemoveIcon />
                </IconButton>
                <IconButton disabled={currHP === props.monster.MaxHitPoints} onClick={() => setHP(currHP + 1)}>
                    <AddIcon />
                </IconButton>
                {currHP}
                /{props.monster.MaxHitPoints}</TableCell>
            <TableCell>
                {props.getActions(props.monster.Tier)}
                <LoopIcon visibility={props.shuffle ? 'visible' : 'hidden'}/>
            </TableCell>
            <TableCell>
                <IconButton>
                    <DeleteIcon />
                </IconButton>
            </TableCell>
        </TableRow>
    );
    function getDefenses() {
            return <div>
                <div style={{ display: (props.monster.BaseShield > 0 ? 'block' : 'none') }}>
                    <ShieldOutlinedIcon /> {props.monster.BaseShield}
                </div>
                <div style={{ display: (props.monster.BaseRetaliate > 0 ? 'block' : 'none') }}>
                    <PanToolOutlinedIcon /> {props.monster.BaseRetaliate}
                    <div style={{ display: (props.monster.BaseRetaliateRange > 1 ? 'block' : 'none') }}>
                        <ArrowCircleRightOutlinedIcon /> {props.monster.BaseRetaliateRange}
                    </div>
                </div>
                <div style={{ display: (props.monster.DoAttackersGainDisadvantage ? 'block' : 'none') }}>
                    Attackers gain Disadvantage
                </div>
            </div>
    }
}