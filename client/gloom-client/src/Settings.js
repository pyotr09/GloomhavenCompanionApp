import React, {useState} from 'react';
import {Drawer, FormControl, Icon, Popover, Stack, TextField} from "@mui/material";
import Button from "@mui/material/Button";

export default function (props) {
    const [sNum, setScenarioNum] = useState('0');
    const [sLevel, setScenarioLevel] = useState('0');

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
        </Drawer>
    );
}