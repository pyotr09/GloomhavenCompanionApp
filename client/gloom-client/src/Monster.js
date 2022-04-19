import React, {useState, useEffect} from "react";
import {Typography} from "@mui/material";

export default function Monster(props) {

    function GetTierText(t) {
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
    
    return (
        <Typography variant="body1" key={props.monster.MonsterNumber}>
            #{props.monster.MonsterNumber} HP: {props.monster.MaxHitPoints}
        </Typography>
    )
}