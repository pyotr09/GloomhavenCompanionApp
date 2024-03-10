import React, {useState} from "react";
import {
    Typography,
    Container,
    Stack,
    TableContainer,
    Paper,
    Table,
    TableRow,
    TableCell,
    TableHead,
    TableBody,
    Grid,
    Tooltip,
    Icon,
    Popover,
    Select,
    MenuItem, FormControl, FormHelperText
} from "@mui/material";
import Button from "@mui/material/Button";
import MonsterGroupRow from "./MonsterGroupRow";
import BossRow from "./BossRow";
import CharacterRow from "./CharacterRow";
import {apiUrl, lockedCharacters, startingCharacters} from "./Constants";

export default function Scenario(props) {

    const [charLevel, setCharLevel] = useState(1);
    const [anchorEl, setAnchorEl] = useState(null);
    const handleCharAddClick = (event) => {
        setAnchorEl(event.currentTarget);
    }

    const handleCharClose = () => {
        setAnchorEl(null);
    };
    const charOpen = Boolean(anchorEl);
    
    const addCharacter = (c) => {
        const body = JSON.stringify(
            {
                "Name": c.Name,
                "Level": charLevel.toString()
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
            `${apiUrl}/${props.sessionId}/addcharacter`,
            init)
            .then(r => r.json())
            .then(json => {
                props.setScenarioState(json);
            });
    }
    
    const setcharinit = (n, i) => {
        const body = JSON.stringify(
            {
                "SessionId": props.sessionId.toString(),
                "Name": n,
                "Initiative": i.toString()
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
            `${apiUrl}/setinitiative`,
            init)
            .then(r => r.json())
            .then(json => {
                props.setScenarioState(json);
            });
    }
    
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

                        <Tooltip title={"click to infuse/consume, right-click to set to waning"}>
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
                                {e.name}</Button>
                        </Tooltip>
                    </Grid>)
                )}
            </Grid>
            <Container>
                <Typography>
                    {props.scenario.name}
                </Typography>
                <Typography>
                    Level: {props.scenario.level}
                </Typography>
                <Button onClick={handleDrawClick}>
                    {props.scenario.isBetweenRounds ? 'Draw' : 'End Round'}
                </Button>
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Initiative</TableCell>
                                <TableCell>Figure</TableCell>
                                <TableCell>Abilities</TableCell>
                                <TableCell>
                                    <Button onClick={(e) => handleCharAddClick(e)}>
                                        Add Character
                                    </Button>
                                    <Popover open={charOpen} anchorEl={anchorEl} onClose={handleCharClose}
                                             anchorOrigin={{vertical: 'bottom', horizontal: 'right'}}
                                    >
                                        <FormControl>
                                        <Select
                                            value={charLevel}
                                            lable={"Level"}
                                            onChange={(e) => setCharLevel(e.target.value)}
                                        >
                                            <MenuItem value={1}>1</MenuItem>
                                            <MenuItem value={2}>2</MenuItem>
                                            <MenuItem value={3}>3</MenuItem>
                                            <MenuItem value={4}>4</MenuItem>
                                            <MenuItem value={5}>5</MenuItem>
                                            <MenuItem value={6}>6</MenuItem>
                                            <MenuItem value={7}>7</MenuItem>
                                            <MenuItem value={8}>8</MenuItem>
                                            <MenuItem value={9}>9</MenuItem>
                                        </Select>
                                            <FormHelperText>Level</FormHelperText>
                                        </FormControl>
                                        <Stack>
                                            {
                                                startingCharacters.map(
                                                    (c) =>
                                                        (<div>
                                                            <Icon>
                                                                <img src={c.Image} alt={c.Name} style={{height: "100%"}}/>
                                                            </Icon>
                                                            <Button key={c.Name} onClick={() => addCharacter(c)} disabled={props.scenario.monsterGroups.some(ch => ch.Name === c.Name)}>
                                                                {c.Name}
                                                            </Button>
                                                        </div>)
                                                )
                                            }
                                            {
                                                lockedCharacters.map((c) =>
                                                    (
                                                        <div>
                                                            <Icon>
                                                                <img src={c.image} alt={c.name} style={{height: "100%"}} disabled={props.scenario.monsterGroups.some(ch => ch.Name === c.Name)}/>
                                                            </Icon>
                                                            <Button key={c.name} disabled>
                                                                ???
                                                            </Button>
                                                        </div>
                                                    )
                                                )
                                            }
                                        </Stack>
                                    </Popover>
                                </TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {props.scenario.monsterGroups.sort(groupCompare).map(
                                (p) => {
                                    // switch (p.type) {
                                    //     case 'Monster':
                                            return <MonsterGroupRow key={p.name} row={p}
                                                                    setScenarioState={props.setScenarioState}
                                                                    scenario={props.scenario}
                                                                    sessionId={props.sessionId}
                                            />;
                                    //     case 'Boss':
                                    //         return <BossRow
                                    //             key={p.Name}
                                    //             boss={p}
                                    //             setScenarioState={props.setScenarioState}
                                    //             scenario={props.scenario}
                                    //             sessionId={props.sessionId}
                                    //         />;
                                    //     case 'Character':
                                    //         return <CharacterRow
                                    //             character={p}
                                    //             setinit={setcharinit}
                                    //             sessionId={props.sessionId}                                           
                                    //         />
                                            
                                    // }
                                }
                            )}
                        </TableBody>
                    </Table>
                </TableContainer>
                
            </Container>
        </div>
    );
    
    function groupCompare(g1, g2) {
        if (g1.initiative === null) {
            return 1;
        }
        if (g2.initiative === null) {
            return -1;
        }
        if (g1.initiative < g2.initiative) {
            return -1;
        }
        if (g1.initiative > g2.initiative) {
            return 1;
        }
        return 0;
    }
    
    function addMonsterToScenario(groupName, monsterJson) {
        const newScenario = props.scenario;
        const groupIndex = newScenario.monsterGroups.findIndex(g => g.Name === groupName);
        newScenario.monsterGroups[groupIndex].monsters.push(monsterJson);
        props.setScenarioState(newScenario);
    }
    
    function handleDrawClick() {
        
        let init = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: ""

        };
        let endpoint = props.scenario.isBetweenRounds ? `drawability` : `endround`;
        fetch(
            `${apiUrl}/${props.sessionId}/${endpoint}`,
            init)
            .then(r => r.json())
            .then(json => {
                props.setScenarioState(json);
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
        // fetch(
        //     `${apiUrl}/${props.sessionid}/setelement`,
        //     init)
        //     .then(r => r.json())
        //     .then(json => {
        //         props.setScenarioState(json);
        //     });
    }

    function getElementButtonVariant(elName) {
        const el = props.scenario.elements[elName];
        if (el === 0) {
            return "outlined";
        }
        if (el === 1 || el === 2) {
            return "contained";
        }
    }

    function getElementButtonStyle(elName, color) {
        const el = props.scenario.elements[elName];
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