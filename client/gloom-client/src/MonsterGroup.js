import {
    Box,
    Card,
    CardActions,
    CardContent, CircularProgress, Divider, Grid, Paper, Stack,
    Typography
} from "@mui/material";
import Monster from "./Monster";
import React, {useEffect, useState} from "react";
import MonsterAdder from "./MonsterAdder";
import LoopIcon from '@mui/icons-material/Loop';
import Image from './images/Monster Ability Card - Front.jpg';
import BackImage from './images/Monster Ability Card - Back.jpg';
import StatsImage from './images/Monster Stat Card v1.png';



export default function (props) {
    const styles = {
        paperFront: {
            backgroundImage: `url('${Image}')`,
            width: 437,
            height: 146
        },
        paperBack: {
            backgroundImage: `url('${BackImage}')`,
            width: 437,
            height: 146
        },
        stats: {
            backgroundImage: `url('${StatsImage}')`,
            width: 395,
            height: 146
        }
    }
    const [loading, setLoading] = useState(false);
    
    const standeeNumbers = Array.from({length: props.group.Count}, (_, i) => i+1);

    console.log(JSON.stringify(props.group))
    return (
        <React.Fragment>
        <Card key={props.group.Name}>
            <CardContent>
                <Typography variant="h5">
                    {props.group.Name + " "}
                </Typography>
                <Stack direction="row" spacing={2}>
                    <Paper style={styles.paperFront}
                           hidden={props.group.ActiveAbilityCard === null || props.group.Monsters.length === 0}>
                        <Stack direction="row" spacing={2}>
                            <Grid item>
                                <Typography mb={2} ml={2} color={"white"} variant={"h3"}>{getInitiative()}</Typography>
                            </Grid>
                            <Grid item>
                                <Stack direction="column" spacing={1}>
                                    <Typography color={"yellow"}>{getEliteActions()}</Typography>
                                    <Typography color={"white"}>{getNormalActions()}</Typography>
                                    {/*need shuffle icon*/}
                                </Stack>
                            </Grid>
                            <Grid item>
                                <LoopIcon visibility={isShuffle() ? 'visible' : 'hidden'}/>
                            </Grid>
                        </Stack>
                    </Paper>
                    <Paper style={styles.paperBack}
                           hidden={props.group.ActiveAbilityCard !== null && props.group.Monsters.length > 0}/>
                    <Paper style={styles.stats}>
                        <Stack>
                            <Typography>{props.group.BaseStatsList[0].Health}</Typography>
                            <Typography>{props.group.BaseStatsList[0].BaseMove}</Typography>
                            <Typography>{parseInt(props.group.BaseStatsList[0].BaseAttackFormula)}</Typography>
                            <Typography>{props.group.BaseStatsList[0].BaseRange}</Typography>
                        </Stack>
                    </Paper>
                </Stack>
                {props.group.Monsters.filter(m => m.Tier === 2).map((m) => (
                            <Monster key={m.MonsterNumber} monster={m} />
                        )
                    )
                }
                <MonsterAdder availableNums={standeeNumbers} tier="elite" add={addMonster} group={props.group} />
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
        </React.Fragment>
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