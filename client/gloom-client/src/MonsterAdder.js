import React, {useEffect, useState} from "react";
import Button from "@mui/material/Button";
import {Dialog, DialogContent, DialogTitle} from "@mui/material";

export default function MonsterAdder(props) {
    const [open, setOpen] = useState(false);

    return (<div>
        <Button size="small" onClick={() => setOpen(true)}>Add {props.tier}</Button>
        <Dialog open={open} onClose={() => setOpen(false)}>
            <DialogTitle>Number?</DialogTitle>
            <DialogContent>
                {props.availableNums.map((n) => (
                    <Button key={n}
                            onClick={() => handleNumButtonClick(n, props.tier)}
                            disabled={props.group.Monsters.some(m => m.MonsterNumber === n)}
                    >
                        {n}
                    </Button>
                ))}
            </DialogContent>
        </Dialog>
    </div>);


    function handleNumButtonClick(n, tier) {
        setOpen(false);
        props.add(tier, n);
    }
}