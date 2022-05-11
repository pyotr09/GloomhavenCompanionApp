import React, {useState} from "react";
import {Icon, IconButton} from "@mui/material";

export default function (props) {
    return(
        <IconButton onClick={() => props.updateStatus(JSON.stringify({[props.alt]: false}))}>
            <Icon>
                <img src={props.image} alt={props.alt} style={{height: "100%"}} />
            </Icon>
        </IconButton>
    );
}