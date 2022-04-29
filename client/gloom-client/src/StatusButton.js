import React, {useState} from "react";
import {Icon, IconButton} from "@mui/material";

export default function (props) {
    return(
        <IconButton onClick={() => props.set(!props.status)}>
            <Icon>
                <img src={props.image} alt={props.alt} style={{height: "100%"}} />
            </Icon>
        </IconButton>
    );
}