import React, {useState, useEffect} from "react";
import {Card, CardHeader, CardContent, Typography, Grid, CardActions, Container} from "@mui/material";
import Button from "@mui/material/Button";
import Monster from "./Monster";
import MonsterGroup from "./MonsterGroup";
import Participant from "./Participant";

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
                    {props.scenario.IsBetweenRounds ? 'Draw' : 'End Round'}
                </Button>
                {props.scenario.MonsterGroups.sort(groupCompare).map(
                    (p) => 
                        <Participant 
                            key={p.Name} 
                            group={p} 
                            addMonster={addMonsterToScenario}
                            setScenarioState={props.setScenarioState}   
                            scenario={props.scenario}
                        />
                )}
                
            </Container>
        </div>
    );
    
    function groupCompare(g1, g2) {
        if (g1.Initiative < g2.Initiative || g2.Initiative === null) {
            return -1;
        }
        if (g1.Initiative > g2.Initiative || g1.Initiative === null) {
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
        let endpoint = props.scenario.IsBetweenRounds ? `drawability` : `endround`;
        fetch(
            `http://127.0.0.1:3000/${endpoint}`,
            init)
            .then(r => r.json())
            .then(json => {
                props.setScenarioState(json);
            });
    }
}