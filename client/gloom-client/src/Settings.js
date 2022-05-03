import React, {useState} from 'react';
import {Drawer, FormControl, Icon, Popover, Stack, TextField} from "@mui/material";
import Button from "@mui/material/Button";
import {lockedCharacters, startingCharacters} from "./Constants";

export default function (props) {
    const [sNum, setScenarioNum] = useState('0');
    const [sLevel, setScenarioLevel] = useState('0');
    const [anchorEl, setAnchorEl] = useState(null);

    const handleCharAddClick = (event) => {
        setAnchorEl(event.currentTarget);
    }

    const handleCharClose = () => {
        setAnchorEl(null);
    };

    const handleClick = (c) => {
        props.addCharacter(
            {
                'Type': 'Character',
                'Initiative': null,
                'Name': c.Name,
                'Image': c.Image,
                'HpByLevel': c.HpByLevel
            }
        )
    }

    const charOpen = Boolean(anchorEl);

    return (
        <Drawer
            anchor={'left'}
            open={props.open}
            onClose={() => props.setOpen(false)}
        >
            <FormControl>
                <TextField margin="normal" inputProps={{inputMode: 'numeric'}} label="Level" type="number"
                           onChange={(e) => setScenarioLevel(e.target.value)}/>
                <TextField margin="normal" inputProps={{inputMode: 'numeric'}} label="Number" type="number"
                           onChange={(e) => setScenarioNum(e.target.value)}/>
                <Button onClick={() => props.setScenarioClicked(sNum, sLevel)}>Set Scenario</Button>
            </FormControl>
            <Button onClick={(e) => handleCharAddClick(e)}>
                Add Characters
            </Button>
            <Popover open={charOpen} anchorEl={anchorEl} onClose={handleCharClose}
                     anchorOrigin={{vertical: 'bottom', horizontal: 'right'}}
            >
                <Stack>
                    {
                        startingCharacters.map(
                            (c) =>
                                (<div>
                                    <Icon>
                                        <img src={c.Image} alt={c.Name} style={{height: "100%"}}/>
                                    </Icon>
                                    <Button key={c.Name} onClick={() => handleClick(c)} disabled={props.characters.some(ch => ch.Name === c.Name)}>
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
                                        <img src={c.Image} alt={c.Name} style={{height: "100%"}} disabled={props.characters.some(ch => ch.Name === c.Name)}/>
                                    </Icon>
                                    <Button key={c.Name}>
                                        ???
                                    </Button>
                                </div>
                            )
                        )
                    }
                </Stack>
            </Popover>
        </Drawer>
    );
}