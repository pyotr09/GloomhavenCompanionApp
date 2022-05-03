import React, {useEffect, useState} from "react";
import Button from "@mui/material/Button";
import AddIcon from "@mui/icons-material/Add";
import {Dialog, DialogContent, DialogTitle, IconButton, Tooltip} from "@mui/material";

export default function MonsterAdder(props) {
    const [open, setOpen] = useState(false);

    return (<div>
        <Tooltip title={"add " + props.tier}>
        <IconButton size="small" onClick={() => setOpen(true)} disabled={props.group.Monsters.length === props.availableNums.length}>
            <AddIcon color={props.tier === "elite" ? "elite" : "inherit"} />
        </IconButton>
        </Tooltip>
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