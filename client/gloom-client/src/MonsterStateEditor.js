import {Checkbox, Grid, Icon, IconButton, Popover, Typography} from "@mui/material";
import React, {useState} from "react";
import RemoveIcon from "@mui/icons-material/Remove";
import AddIcon from "@mui/icons-material/Add";
import HealImage from "./images/Heal.svg";
import DisarmImage from "./images/Disarm.svg";
import BlessImage from "./images/Bless.svg";
import CurseImage from "./images/Curse.svg";
import ImmobilizeImage from "./images/Immobilize.svg";
import MuddleImage from "./images/Muddle.svg";
import PoisonImage from "./images/Poison.svg";
import StrengthenImage from "./images/Strengthen.svg";
import StunImage from "./images/Stun.svg";
import WoundImage from "./images/Wound.svg";
import InvisibleImage from "./images/Invisible.svg";

export default function (props) {
    const [disarm, setDisarm] = useState(props.statuses.disarm.isActive);
    const [stun, setStun] = useState(props.statuses.stun.isActive);
    const [immobilize, setImmobilize] = useState(props.statuses.immobilize.isActive);
    const [muddle, setMuddle] = useState(props.statuses.muddle.isActive);
    const [poison, setPoison] = useState(props.statuses.poison.isActive);
    const [wound, setWound] = useState(props.statuses.wound.isActive);
    const [strengthen, setStrengthen] = useState(props.statuses.strengthen.isActive);
    const [invisible, setInvisible] = useState(props.statuses.invisible.isActive);

    const statuses = [
        {status: disarm, set: setDisarm, image: DisarmImage, alt: "disarm"},
        {status: stun, set: setStun, image: StunImage, alt: "stun"},
        {status: immobilize, set: setImmobilize, image: ImmobilizeImage, alt: "immobilize"},
        {status: muddle, set: setMuddle, image: MuddleImage, alt: "muddle"},
        {status: poison, set: setPoison, image: PoisonImage, alt: "poison"},
        {status: wound, set: setWound, image: WoundImage, alt: "wound"},
        {status: strengthen, set: setStrengthen, image: StrengthenImage, alt: "strengthen"},
        {status: invisible, set: setInvisible, image: InvisibleImage, alt: "invisible"},
    ]
    
    const[hp, setHp] = useState(props.currentHP);
    const open = Boolean(props.anchorEl);
    
    const handleStatusClose = () => {
        props.handleStatusClose(hp, {
            "Disarm": disarm,
            "Stun": stun,
            "Immobilize": immobilize,
            "Muddle": muddle,
            "Poison": poison,
            "Wound": wound,
            "Strengthen": strengthen
        });
    }
    
    return (
        <Popover
            open={open}
            anchorEl={props.anchorEl}
            onClose={handleStatusClose}
            anchorOrigin={{
                vertical: 'bottom',
                horizontal: 'left',
            }}
        >
            <Grid container justifyContent={"center"} spacing={1}>
                <Grid item xs={2}>               
                    <IconButton disabled={hp === 0} onClick={() => setHp(hp - 1)}>
                        <RemoveIcon />
                    </IconButton>
                </Grid>
                <Grid item xs={2}>
                    <Icon>
                        <img src={HealImage} alt={"Health"} style={{height: "100%"}} />
                    </Icon>
                    <Typography variant="caption">{hp}</Typography>
                </Grid>
                <Grid item xs={2}>
                    <IconButton disabled={hp === props.maxHp} onClick={() => setHp(hp + 1)}>
                        <AddIcon />
                    </IconButton>
                </Grid>
                {statuses.map((s) =>
                    (
                        <Grid item  xs={2} key={s.alt}>
                            <Icon><img src={s.image} alt={s.alt} style={{height: "100%"}} /></Icon>
                            :
                            <Checkbox checked={s.status} onChange={() => s.set(!s.status)} />
                        </Grid>)
                )}
            </Grid>
        </Popover>);
}