import React, {useState} from "react";
import {Dialog, DialogContent, DialogTitle, TableCell, TableRow, TextField} from "@mui/material";
import Button from "@mui/material/Button";

export default function (props) {
    const [initPickerOpen, setInitOpen] = useState(false);
    
    const setInit = (n) => {
        const newChar = props.character;
        newChar.Initiative = n;
        props.updateCharState(newChar);
    }
    
    return (
        <React.Fragment>
            <TableRow>
                <TableCell>
                    <Button onClick={() => setInitOpen(true)}>
                        {props.character.Initiative ?? "?"}
                    </Button>
                </TableCell>
                <TableCell>{props.character.Name}</TableCell>
                <TableCell />
            </TableRow>
            <Dialog open={initPickerOpen} onClose={() => setInitOpen(false)}>
                <DialogTitle>Set Initiative</DialogTitle>
                <DialogContent>
                    <TextField margin="normal" inputProps={{inputMode: 'numeric'}} label="Initiaitive" type="number"
                               onChange={(e) => setInit(e.target.value)}/>
                </DialogContent>
            </Dialog>
        </React.Fragment>
    );
}