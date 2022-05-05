import {
    Grid, Icon,
    IconButton,
    Popover,
    TableCell,
    TableRow, Tooltip, Typography
} from "@mui/material";
import React, {useState} from "react";
import AddIcon from "@mui/icons-material/Add";
import RemoveIcon from "@mui/icons-material/Remove";
import DeleteIcon from "@mui/icons-material/Delete";
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
import StatusButton from "./StatusButton";
import ShuffleImage from "./images/Shuffle.svg";
import ShieldImage from "./images/Shield.svg";
import RetaliateImage from "./images/Retaliate.svg";
import RangeImage from "./images/Range.svg";

export default function(props) {
    const [currHP, setHP] = useState(props.monster.CurrentHitPoints);
    const [anchorEl, setAnchorEl] = useState(null);
    const [disarm, setDisarm] = useState(props.monster.Statuses.Disarm.IsActive);
    const [stun, setStun] = useState(props.monster.Statuses.Stun.IsActive);
    const [immobilize, setImmobilize] = useState(props.monster.Statuses.Immobilize.IsActive);
    const [muddle, setMuddle] = useState(props.monster.Statuses.Muddle.IsActive);
    const [poison, setPoison] = useState(props.monster.Statuses.Poison.IsActive);
    const [wound, setWound] = useState(props.monster.Statuses.Wound.IsActive);
    const [strengthen, setStrengthen] = useState(props.monster.Statuses.Strengthen.IsActive);
    const [invisible, setInvisible] = useState(props.monster.Statuses.Invisible.IsActive);
    
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
    
    const handleStatusAddClick = (event) => {
        setAnchorEl(event.currentTarget);
    }

    const handleStatusClose = () => {
        setAnchorEl(null);
    };

    const open = Boolean(anchorEl);
    
    return(
        <TableRow>
            <TableCell>
                <Typography color={props.monster.Tier === 2 ?  "#fbc02d" : ""}>
                    #{props.monster.MonsterNumber}
                </Typography>
            </TableCell>
            <TableCell align={"center"}>
                <Grid container justifyContent={"center"}>
                    {statuses.map((s) =>                     
                        (s.status ? <Grid item key={s.alt}><StatusButton set={s.set} image={s.image} status={s.status} alt={s.alt}/></Grid> : "")
                    )}
                </Grid>
                
                <IconButton onClick={(e) => handleStatusAddClick(e)}>
                    <AddIcon />
                </IconButton>
                <Popover 
                    open={open}
                    anchorEl={anchorEl}
                    onClose={handleStatusClose}
                    anchorOrigin={{
                        vertical: 'bottom',
                        horizontal: 'left',
                    }}
                >
                    <Grid container justifyContent={"center"}>
                        {statuses.map((s) =>
                            (<Grid item key={s.alt}><StatusButton set={s.set} image={s.image} status={s.status} alt={s.alt}/></Grid>)
                        )}
                    </Grid>
                </Popover>
            </TableCell>
            <TableCell>
                {getDefenses()}
            </TableCell>
            <TableCell>
                {currHP}
                /{props.monster.MaxHitPoints}
                <IconButton disabled={currHP === 0} onClick={() => setHP(currHP - 1)}>
                    <RemoveIcon />
                </IconButton>
                <IconButton disabled={currHP === props.monster.MaxHitPoints} onClick={() => setHP(currHP + 1)}>
                    <AddIcon />
                </IconButton></TableCell>
            <TableCell>
                {props.getActions(props.monster.Tier)}
                {props.shuffle ?
                    <Icon>
                        <img src={ShuffleImage} alt={props.alt} style={{height: "100%"}} />
                    </Icon>
                    : ""
                }
            </TableCell>
            <TableCell>
                <IconButton onClick={() => props.removeMonster(props.monster.MonsterNumber)}>
                    <DeleteIcon />
                </IconButton>
            </TableCell>
        </TableRow>
    );
    function getDefenses() {
            return <div>
                {props.monster.BaseShield > 0 ? 
                <Typography>
                    <Tooltip title="Shield">
                        <Icon>
                            <img src={ShieldImage} alt={props.alt} style={{height: "100%"}} />
                        </Icon>
                    </Tooltip>
                    {props.monster.BaseShield}
                </Typography> : 
                    "" }
                {props.monster.BaseRetaliate > 0 ? 
                <Typography>
                    <Tooltip title="Retaliate">
                        <Icon>
                            <img src={RetaliateImage} alt={props.alt} style={{height: "100%"}} />
                        </Icon>
                    </Tooltip>
                    {props.monster.BaseRetaliate}
                    {props.monster.BaseRetaliateRange > 1 ? 
                    <Typography>
                        <Tooltip title="Retaliate Range">
                            <Icon>
                                <img src={RangeImage} alt={props.alt} style={{height: "100%"}} />
                            </Icon>
                        </Tooltip>
                        {props.monster.BaseRetaliateRange}
                    </Typography> : ""}
                </Typography>: "" }
                <div style={{ display: (props.monster.DoAttackersGainDisadvantage ? 'block' : 'none') }}>
                    Attackers gain Disadvantage
                </div>
            </div>
    }
}