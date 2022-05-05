import React, {useState} from "react";
import {
    Typography,
    Container,
    Stack,
    TableContainer, Paper, Table, TableRow, TableCell, TableHead, TableBody, Grid
} from "@mui/material";
import Button from "@mui/material/Button";
import Monster from "./Monster";
import MonsterGroupRow from "./MonsterGroupRow";
import BossRow from "./BossRow";
import CharacterRow from "./CharacterRow";

export default function Scenario(props) {

    const elements = [
        {color: "#d81b60", paletteName: "fire", name: "Fire"},
        {color: "#4fc3f7", paletteName: "ice", name: "Ice"},
        {color: "#43a047", paletteName: "earth", name: "Earth"},
        {color: "#757575", paletteName: "air", name: "Air"},
        {color: "#fdd835", paletteName: "light", name: "Light"},
        {color: "#212121", paletteName: "dark", name: "Dark"}
    ];
    
    return (
        <div>
            <Grid container spacing={2} p={2} justifyContent={"center"}>
                {elements.map((e) =>
                    (<Grid item
                           key={e.name}>
                        <Button
                        style={getElementButtonStyle(e.name, e.color)}
                        variant={getElementButtonVariant(e.name)}
                        color={e.paletteName}
                        onClick={() => {
                            setElement(e.name, false)
                        }}
                        onContextMenu={ (ev) => {
                            ev.preventDefault();
                            setElement(e.name, true)}}
                    >
                        {e.name}
                    </Button>
                    </Grid>)
                )}
            </Grid>
            <Container>
                <Typography>
                    {props.scenario.Name}
                </Typography>
                <Typography>
                    Level: {props.scenario.Level}
                </Typography>
                <Button onClick={handleDrawClick}>
                    {props.scenario.IsBetweenRounds ? 'Draw' : 'End Round'}
                </Button>
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Initiative</TableCell>
                                <TableCell>Figure</TableCell>
                                <TableCell>Abilities</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {props.scenario.MonsterGroups.concat(props.characters).sort(groupCompare).map(
                                (p) => {
                                    switch (p.Type) {
                                        case 'Monster':
                                            return <MonsterGroupRow key={p.Name} row={p}
                                                                    setScenarioState={props.setScenarioState}
                                                                    scenario={props.scenario}
                                                                    sessionId={props.sessionId}
                                            />;
                                        case 'Boss':
                                            return <BossRow
                                                key={p.Name}
                                                boss={p}
                                                setScenarioState={props.setScenarioState}
                                                scenario={props.scenario}
                                                sessionId={props.sessionId}
                                            />;
                                        case 'Character':
                                            return <CharacterRow
                                                character={p}
                                                updateCharState={props.updateCharState}
                                                sessionId={props.sessionId}                                           
                                            />
                                            
                                    }
                                }
                            )}
                        </TableBody>
                    </Table>
                </TableContainer>
                
            </Container>
        </div>
    );
    
    function groupCompare(g1, g2) {
        if (g1.Initiative === null) {
            return 1;
        }
        if (g2.Initiative === null) {
            return -1;
        }
        if (g1.Initiative < g2.Initiative) {
            return -1;
        }
        if (g1.Initiative > g2.Initiative) {
            return 1;
        }
        return 0;
    }
    
    function addMonsterToScenario(groupName, monsterJson) {
        const newScenario = props.scenario;
        const groupIndex = newScenario.MonsterGroups.findIndex(g => g.Name === groupName);
        newScenario.MonsterGroups[groupIndex].Monsters.push(monsterJson);
        props.setScenarioState(newScenario);
    }
    
    function handleDrawClick() {
        const body = JSON.stringify(
            {
                "SessionId": props.sessionId
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
                if (endpoint === `endround`) {
                    //reduceElements()
                }
            });
    }
    
    function setElement(elName, isWaning) {
        const body = JSON.stringify(
            {
                "SessionId": props.sessionId,
                "Element": elName,
                "SetWaning": isWaning.toString()
            }
        );
        const init = {
            method: 'POST',
            headers: {
                'Content-Type': 'text/plain'
            },
            body: body
        };
        fetch(
            `http://127.0.0.1:3000/setelement`,
            init)
            .then(r => r.json())
            .then(json => {
                props.setScenarioState(json);
            });
    }

    function getElementButtonVariant(elName) {
        const el = props.scenario.Elements[elName];
        if (el === 0) {
            return "outlined";
        }
        if (el === 1 || el === 2) {
            return "contained";
        }
    }

    function getElementButtonStyle(elName, color) {
        const el = props.scenario.Elements[elName];
        if (el === 0 || el === 2) {
            return {};
        }
        if (el === 1) {
            return {
                background: `linear-gradient(to bottom, Transparent 0%,Transparent 50%,${color} 50%,${color} 100%)`
            };
        }
    }
}