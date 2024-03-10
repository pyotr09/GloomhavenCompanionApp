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
import disarmImage from "./images/Disarm.svg";
import BlessImage from "./images/Bless.svg";
import CurseImage from "./images/Curse.svg";
import immobilizeImage from "./images/Immobilize.svg";
import muddleImage from "./images/Muddle.svg";
import poisonImage from "./images/Poison.svg";
import strengthenImage from "./images/Strengthen.svg";
import stunImage from "./images/Stun.svg";
import woundImage from "./images/Wound.svg";
import invisibleImage from "./images/Invisible.svg";
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
    
    const disarm = props.monster.statuses.disarm.isActive;
    const stun = props.monster.statuses.stun.isActive;
    const immobilize  = (props.monster.statuses.immobilize.isActive);
    const muddle = (props.monster.statuses.muddle.isActive);
    const poison = (props.monster.statuses.poison.isActive);
    const wound = (props.monster.statuses.wound.isActive);
    const strengthen = (props.monster.statuses.strengthen.isActive);
    const invisible = (props.monster.statuses.invisible.isActive);
    
    const statuses = [
        {status: disarm, image: disarmImage, alt: "disarm"},
        {status: stun, image: stunImage, alt: "stun"},
        {status: immobilize, image: immobilizeImage, alt: "immobilize"},
        {status: muddle, image: muddleImage, alt: "muddle"},
        {status: poison, image: poisonImage, alt: "poison"},
        {status: wound, image: woundImage, alt: "wound"},
        {status: strengthen, image: strengthenImage, alt: "strengthen"},
        {status: invisible, image: invisibleImage, alt: "invisible"},
    ]
    
    const handleStatusAddClick = (event) => {
        setAnchorEl(event.currentTarget);
    }
    
    const updateMonsterState = (hp, statusesObject) => {
        const body = JSON.stringify(
            {
                "GroupName": props.groupName,
                "MonsterNumber": props.monster.monsterNumber,
                "NewHp": hp,
                "statuses": statusesObject
                // todo bless and curse numbers
            }
        );
        const init = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: body
        };
        fetch(
            `${apiUrl}/${props.sessionId}/updatemonsterstate`,
            init)
            .then(r => r.json())
            .then(json => {
                props.setScenarioState(json);
            });
    }

    const handleStatusClose = (hp, statusesObject) => {
        updateMonsterState(hp, statusesObject);
        setAnchorEl(null);
    };
    
    return(
        <TableRow>
            <TableCell>
                <Typography color={props.monster.Tier === 2 ?  "#fbc02d" : ""}>
                    #{props.monster.monsterNumber}
                    <IconButton onClick={(e) => handleStatusAddClick(e)}>
                        <EditIcon />
                    </IconButton>
                    {anchorEl != null ? 
                    <MonsterStateEditor
                        anchorEl={anchorEl}
                        statuses={props.monster.statuses}
                        handleStatusClose={handleStatusClose}
                        currentHP={props.monster.currentHitPoints}
                        maxHp={props.monster.maxHitPoints}
                     /> : ""}
                </Typography>
            </TableCell>
            <TableCell align={"center"}>
                <Grid container justifyContent={"center"}>
                    {statuses.map((s) =>                     
                        (s.status ? <Grid item key={s.alt}>
                            <StatusButton 
                                updateStatus={(statusesString) => updateMonsterState(props.monster.currentHitPoints, statusesString)}
                                image={s.image} status={s.status} alt={s.alt}/>
                        </Grid> : "")
                    )}
                </Grid>
            </TableCell>
            <TableCell>
                {getDefenses()}
            </TableCell>
            <TableCell>
                {props.monster.currentHitPoints}
                /{props.monster.maxHitPoints}</TableCell>
            <TableCell>
                {props.getActions(props.monster.tier)}
                {props.shuffle ?
                    <Icon>
                        <img src={ShuffleImage} alt={props.alt} style={{height: "100%"}} />
                    </Icon>
                    : ""
                }
            </TableCell>
            <TableCell>
                <IconButton onClick={() => props.removeMonster(props.monster.monsterNumber)}>
                    <DeleteIcon />
                </IconButton>
            </TableCell>
        </TableRow>
    );
    function getDefenses() {
            return <div>
                {props.monster.baseShield > 0 ? 
                <Typography>
                    <Tooltip title="Shield">
                        <Icon>
                            <img src={ShieldImage} alt={props.alt} style={{height: "100%"}} />
                        </Icon>
                    </Tooltip>
                    {props.monster.baseShield}
                </Typography> : 
                    "" }
                {props.monster.baseRetaliate > 0 ? 
                <Typography>
                    <Tooltip title="Retaliate">
                        <Icon>
                            <img src={RetaliateImage} alt={props.alt} style={{height: "100%"}} />
                        </Icon>
                    </Tooltip>
                    {props.monster.baseRetaliate}
                    {props.monster.baseRetaliateRange > 1 ? 
                    <Typography>
                        <Tooltip title="Retaliate Range">
                            <Icon>
                                <img src={RangeImage} alt={props.alt} style={{height: "100%"}} />
                            </Icon>
                        </Tooltip>
                        {props.monster.baseRetaliateRange}
                    </Typography> : ""}
                </Typography>: "" }
                <div style={{ display: (props.monster.doAttackersGainDisadvantage ? 'block' : 'none') }}>
                    Attackers gain Disadvantage
                </div>
            </div>
    }
}