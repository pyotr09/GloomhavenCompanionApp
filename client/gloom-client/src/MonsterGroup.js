import {
    Card,
    CardActions,
    CardContent, CircularProgress, Divider,
    Typography
} from "@mui/material";
import Monster from "./Monster";
import React, {useEffect, useState} from "react";
import MonsterAdder from "./MonsterAdder";

export default function (props) {
    const [availableNums, setAvailableNums] = useState([1,2,3,4,5,6]);
    const [loading, setLoading] = useState(false);

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
                <Typography>
                    {getEliteActions()}
                </Typography>
                {props.group.Monsters.filter(m => m.Tier === 2).map((m) => (
                            <Monster key={m.MonsterNumber} monster={m} />
                        )
                    )
                }
                <MonsterAdder availableNums={availableNums} tier="elite" add={addMonster} />
                <Divider />
                <Typography
                    sx={{ mt: 0.5, ml: 2 }}
                    color="text.secondary"
                    display="block"
                    variant="caption"
                >
                    Normal
                </Typography>
                <Typography>
                    {getNormalActions()}
                </Typography>
                {props.group.Monsters.filter(m => m.Tier === 3).map((m) => (
                            <Monster key={m.MonsterNumber} monster={m} />
                        )
                    )
                }
                <MonsterAdder availableNums={availableNums} tier="normal" add={addMonster} />
            </CardContent>
            <CardActions>
            </CardActions>
        </Card>
    );
    
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
        setAvailableNums(availableNums.filter(n => n !== num));
        makeMonsterAPICall(tier, props.group.Name, num);
    }
    
    function finished() {
        setLoading(false);
    }

    function makeMonsterAPICall(tier, name, num) {
        const body = JSON.stringify(
            {
                "Level": props.level.toString(),
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
                props.addMonster(props.group.Name, json);
                // let newMonsters = monsters;
                // newMonsters.push(json);
                // setMonsters(newMonsters);
                finished();
            });
    }
}