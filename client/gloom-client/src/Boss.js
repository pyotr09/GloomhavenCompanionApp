import React, {useEffect, useState} from "react";
import {Card, CardActions, CardContent, Divider, Typography} from "@mui/material";
import LoopIcon from "@mui/icons-material/Loop";
import Button from "@mui/material/Button";

export default function (props) {
    return (
        <Card key={props.boss.Name}>
            <CardContent>
                <Typography variant="h5">
                    {props.boss.Name + " " + getInitiative()}
                </Typography>
                <Divider />
                <Typography hidden={props.boss.ActiveAbilityCard === null}>
                    {getActions()}
                    <LoopIcon visibility={isShuffle() ? 'visible' : 'hidden'} />
                </Typography>
                <Typography variant="body1" hidden={!props.boss.IsActive}>
                    HP: {props.boss.MaxHealth}
                </Typography>
                <Button disabled={props.boss.IsActive} onClick={handleAdd}>Add Boss</Button>
            </CardContent>
            <CardActions>
            </CardActions>
        </Card>
    );

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