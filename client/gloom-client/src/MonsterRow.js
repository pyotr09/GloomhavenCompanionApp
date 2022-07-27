import {
    Grid, Icon,
    IconButton,
    Popover,
    TableCell,
    TableRow, Tooltip, Typography
} from "@mui/material";
import React, {useState} from "react";
import EditIcon from "@mui/icons-material/Edit";
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
import MonsterStateEditor from "./MonsterStateEditor";
import {Update} from "@mui/icons-material";
import {apiUrl} from "./Constants";

export default function(props) {
    const [anchorEl, setAnchorEl] = useState(null);
    
    const disarm = props.monster.Statuses.Disarm.IsActive;
    const stun = props.monster.Statuses.Stun.IsActive;
    const immobilize  = (props.monster.Statuses.Immobilize.IsActive);
    const muddle = (props.monster.Statuses.Muddle.IsActive);
    const poison = (props.monster.Statuses.Poison.IsActive);
    const wound = (props.monster.Statuses.Wound.IsActive);
    const strengthen = (props.monster.Statuses.Strengthen.IsActive);
    const invisible = (props.monster.Statuses.Invisible.IsActive);
    
    const statuses = [
        {status: disarm, image: DisarmImage, alt: "Disarm"},
        {status: stun, image: StunImage, alt: "Stun"},
        {status: immobilize, image: ImmobilizeImage, alt: "Immobilize"},
        {status: muddle, image: MuddleImage, alt: "Muddle"},
        {status: poison, image: PoisonImage, alt: "Poison"},
        {status: wound, image: WoundImage, alt: "Wound"},
        {status: strengthen, image: StrengthenImage, alt: "Strengthen"},
        {status: invisible, image: InvisibleImage, alt: "Invisible"},
    ]
    
    const handleStatusAddClick = (event) => {
        setAnchorEl(event.currentTarget);
    }
    
    const updateMonsterState = (hp, statusesString) => {
        const body = JSON.stringify(
            {
                "SessionId": props.sessionId.toString(),
                "GroupName": props.groupName,
                "MonsterNumber": props.monster.MonsterNumber.toString(),
                "NewHp": hp.toString(),
                "Statuses": statusesString
                // todo bless and curse numbers
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
            `${apiUrl}/updatemonsterstate`,
            init)
            .then(r => r.json())
            .then(json => {
                props.setScenarioState(json);
            });
    }

    const handleStatusClose = (hp, statusesString) => {
        updateMonsterState(hp, statusesString);
        setAnchorEl(null);
    };
    
    return(
        <TableRow>
            <TableCell>
                <Typography color={props.monster.Tier === 2 ?  "#fbc02d" : ""}>
                    #{props.monster.MonsterNumber}
                    <IconButton onClick={(e) => handleStatusAddClick(e)}>
                        <EditIcon />
                    </IconButton>
                    {anchorEl != null ? 
                    <MonsterStateEditor
                        anchorEl={anchorEl}
                        Statuses={props.monster.Statuses}
                        handleStatusClose={handleStatusClose}
                        currentHP={props.monster.CurrentHitPoints}
                        maxHp={props.monster.MaxHitPoints}
                     /> : ""}
                </Typography>
            </TableCell>
            <TableCell align={"center"}>
                <Grid container justifyContent={"center"}>
                    {statuses.map((s) =>                     
                        (s.status ? <Grid item key={s.alt}>
                            <StatusButton 
                                updateStatus={(statusesString) => updateMonsterState(props.monster.CurrentHitPoints, statusesString)}
                                image={s.image} status={s.status} alt={s.alt}/>
                        </Grid> : "")
                    )}
                </Grid>
            </TableCell>
            <TableCell>
                {getDefenses()}
            </TableCell>
            <TableCell>
                {props.monster.CurrentHitPoints}
                /{props.monster.MaxHitPoints}</TableCell>
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