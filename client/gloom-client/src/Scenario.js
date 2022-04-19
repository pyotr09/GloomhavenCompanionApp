import React, {useState, useEffect} from "react";
import {Card, CardHeader, CardContent, Typography, Grid, CardActions, Container} from "@mui/material";
import Button from "@mui/material/Button";
import Monster from "./Monster";
import MonsterGroup from "./MonsterGroup";

export default function Scenario(props) {
    
    return (
        <div>
            <Container maxWidth="md">
                <Typography>
                    {props.scenario.Name}
                </Typography>
                <Typography>
                    Level: {props.scenario.Level}
                </Typography>
                <Button onClick={handleDrawClick}>
                    Draw
                </Button>
                {props.scenario.MonsterGroups.sort(groupCompare).map(
                    (p) => <MonsterGroup key={p.Name} group={p} level={props.scenario.Level} addMonster={addMonsterToScenario}/>
                )}
                
            </Container>
        </div>
    );
    
    function groupCompare(g1, g2) {
        if (g1.Initiative < g2.Initiative || g2.Initiative === null) {
            console.log("returned: " + -1)
            return -1;
        }
        if (g1.Initiative > g2.Initiative || g1.Initiative === null) {
            console.log("returned: " + 1)
            return 1;
        }
        return 0;
    }
    
    function addMonsterToScenario(groupName, monsterJson) {
        let newScenario = props.scenario;
        let groupIndex = newScenario.MonsterGroups.findIndex(g => g.Name === groupName);
        newScenario.MonsterGroups[groupIndex].Monsters.push(monsterJson);
        props.setScenarioState(newScenario);
    }
    
    function handleDrawClick() {
        const body = JSON.stringify(
            {
                "PreviousState": JSON.stringify(props.scenario)
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
            `http://127.0.0.1:3000/drawability`,
            init)
            .then(r => r.json())
            .then(json => {
                props.setScenarioState(json);
            });
    }
}