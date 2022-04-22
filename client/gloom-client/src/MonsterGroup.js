import {
    Card,
    CardActions,
    CardContent, CircularProgress, Divider,
    Typography
} from "@mui/material";
import Monster from "./Monster";
import React, {useEffect, useState} from "react";
import MonsterAdder from "./MonsterAdder";
import LoopIcon from '@mui/icons-material/Loop';

export default function (props) {
    const [loading, setLoading] = useState(false);
    
    const standeeNumbers = Array.from({length: props.group.Count}, (_, i) => i+1);

    return (
        <Card key={props.group.Name}>
            <CardContent>
                <Typography variant="h5">
                    {props.group.Name + " " + getInitiative()}
                </Typography>
                <Divider />
                <Typography
                    sx={{ mt: 0.5, ml: 2 }}
                    color="text.secondary"
                    display="block"
                    variant="caption"
                >
                    Elite
                </Typography>
                <Typography hidden={props.group.ActiveAbilityCard === null || !props.group.Monsters.some(m => m.Tier === 2)}>
                    {getEliteActions()}
                    <LoopIcon visibility={isShuffle() ? 'visible' : 'hidden'} />
                </Typography>
                {props.group.Monsters.filter(m => m.Tier === 2).map((m) => (
                            <Monster key={m.MonsterNumber} monster={m} />
                        )
                    )
                }
                <MonsterAdder availableNums={standeeNumbers} tier="elite" add={addMonster} group={props.group} />
                <Divider />
                <Typography
                    sx={{ mt: 0.5, ml: 2 }}
                    color="text.secondary"
                    display="block"
                    variant="caption"
                >
                    Normal
                </Typography>
                <Typography hidden={props.group.ActiveAbilityCard === null || !props.group.Monsters.some(m => m.Tier === 3)}>
                    {getNormalActions()}
                    <LoopIcon visibility={isShuffle() ? 'visible' : 'hidden'} />
                </Typography>
                {props.group.Monsters.filter(m => m.Tier === 3).map((m) => (
                            <Monster key={m.MonsterNumber} monster={m} />
                        )
                    )
                }
                <MonsterAdder availableNums={standeeNumbers} tier="normal" add={addMonster} group={props.group} />
            </CardContent>
            <CardActions>
            </CardActions>
        </Card>
    );
    
    function isShuffle() {
        if (props.group.ActiveAbilityCard === null)
            return false;
        return props.group.ActiveAbilityCard.ShuffleAfter;
    }
    
    function getInitiative() {
        if (props.group.Initiative === null)
            return "";
        else return parseInt(props.group.Initiative);
    }
    
    function getEliteActions() {
        if (props.group.ActiveAbilityCard === null)
            return "";
        return props.group.ActiveAbilityCard.Actions.map(a => a.EliteActionText).join(", ");
    }
    
    function getNormalActions() {
        if (props.group.ActiveAbilityCard === null)
            return "";
        return props.group.ActiveAbilityCard.Actions.map(a => a.NormalActionText).join(", ");
        
    }
    
    function addMonster(tier, num) {
        setLoading(true);
        makeMonsterAPICall(tier, props.group.Name, num);
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
                const ogCount = props.group.Monsters.length;
                props.addMonster(props.group.Name, json);     
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
                "GroupName": props.group.Name
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