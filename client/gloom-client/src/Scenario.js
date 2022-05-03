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
    const [fire, setFire] = useState(0);
    const [ice, setIce] = useState(0);
    const [earth, setEarth] = useState(0);
    const [air, setAir] = useState(0);
    const [light, setLight] = useState(0);
    const [dark, setDark] = useState(0);

    const elements = [
        {el: fire, setEl: setFire, color: "#d81b60", paletteName: "fire", name: "FIRE"},
        {el: ice, setEl: setIce, color: "#4fc3f7", paletteName: "ice", name: "ICE"},
        {el: earth, setEl: setEarth, color: "#43a047", paletteName: "earth", name: "EARTH"},
        {el: air, setEl: setAir, color: "#757575", paletteName: "air", name: "AIR"},
        {el: light, setEl: setLight, color: "#fdd835", paletteName: "light", name: "LIGHT"},
        {el: dark, setEl: setDark, color: "#212121", paletteName: "dark", name: "DARK"}
    ];
    
    return (
        <div>
            <Grid container spacing={2} p={2} justifyContent={"center"}>
                {elements.map((e) =>
                    (<Grid item
                           key={e.name}>
                        <Button
                        style={getElementButtonStyle(e.el, e.color)}
                        variant={getElementButtonVariant(e.el)}
                        color={e.paletteName}
                        onClick={() => {
                            if (e.el === 0) e.setEl(2); else e.setEl(0);
                        }}
                        onDoubleClick={() => e.setEl(1)}
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
                    reduceElements()
                }
            });
    }

    function reduceElements() {
        if (fire > 0)
            setFire(fire - 1);
        if (ice > 0)
            setIce(ice - 1);
        if (earth > 0)
            setEarth(earth - 1);
        if (air > 0)
            setAir(air - 1);
        if (light > 0)
            setLight(light - 1);
        if (dark > 0)
            setDark(dark - 1);
    }

    function getElementButtonVariant(el) {
        if (el === 0) {
            return "outlined";
        }
        if (el === 1 || el === 2) {
            return "contained";
        }
    }

    function getElementButtonStyle(el, color) {
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