import MenuIcon from "@mui/icons-material/Menu";
import AddIcon from "@mui/icons-material/Add";
import {Typography} from "@mui/material";
import React from "react";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";

export default function () {
    return (<Typography variant="body1" gutterBottom style={{whiteSpace: "pre"}}>
        Current Version: v0.1{'\n'}
        {'\n'}
        How to Use{'\n'}
        - Click START NEW SESSION{'\n'}
        - Take note of the Session Id{'\n'}
        - To join a session already started on another device or resume a previous session{'\n'}
        {'  '}- Click JOIN SESSSION and enter the Session Id{'\n'}
        - After starting or joining a session, if you'd like to start or join a different one, reload the page{'\n'}
        {'\n'}
        - Click <MenuIcon /> to initiate a scenario{'\n'}
        - Enter the scenario level and number (base Gloomhaven only){'\n'}
        {'\n'}
        - Click ADD CHARACTER to add your characters to the scenario (Gloomhaven starter classes only){'\n'}
        - Click <AddIcon color={"elite"} /> to add an elite monster of that type and <AddIcon color={"inherit"} /> to add a normal{'\n'}
        {'\n'}
        - Enter Initiative value for each character by clicking the ? under Initiative{'\n'}
        - Once all Initiative values are entered, click DRAW to begin the next turn{'\n'}
        - Characters and monster groups will be ordered by initiative{'\n'}
        {'\n'}
        - The drawn monster ability will be displayed under Abilities for that monster group{'\n'}
        - The ability with calculated attack/move/range values and innate effects will be displayed next to each elite or normal monster entry{'\n'}
        - Defense values such as shield and retaliate will be displayed under DEF{'\n'}
        {'\n'}
        - Click <EditIcon /> to adjust a figures's health or active statuses{'\n'}
        - You can also click the status within the table row to remove it{'\n'}
        - Click <DeleteIcon /> to remove a figure{'\n'}
        {'\n'}
        - Click an element button to infuse or consume that element. Right-click (long press on mobile) to set it to waning.{'\n'}
        {'\n'}
        - Click END ROUND when all figures have taken their turn{'\n'}
        - Elements will decrease{'\n'}
        - Monster ability will be hidden until next DRAW{'\n'}
        - Character initiatives will be reset to ?{'\n'}
        {'\n'}
        Known Issues{'\n'}
        - App will occasionally not perist changes.{'\n'}
        {'  '}- If you make some changes but it reverts to a previous state, wait a few minutes before trying another change.{'\n'}
        {'  '}- The database is updated asynchronously and may not be handling collisions efficiently{'\n'}
        - Pre-1.0, the database will be periodically wiped out{'\n'}
        {'\n'}
        Feature Roadmap:{'\n'}
        {'\n'}
        v1.0{'\n'}
        - Include these notes in a help section{'\n'}
        - Track health and statuses on characters{'\n'}
        - Implement locked characters{'\n'}
        - Monster Attack Modifier Deck (draw, maintain discard pile, manage bless/curse, auto shuffle){'\n'}
        - With multiple devices connected to the same session, automatically refresh all connected states when a change happens.{'\n'}
        {'  '}- Before 1.0, use the Refresh button at the top of the screen to update your device's state with other devices' changes.{'\n'}
        - Ability to add monsters not included in the scenario (ie. summoned monsters or custom scenarios){'\n'}
        - Ability to track character summons{'\n'}
        - Replace text for Monster Abilites' area of effect with the correct visual (red hexes etc.){'\n'}
        - Enforce all characters to have initiative set before allowing "draw" to be selected{'\n'}
        {'\n'}
        v1.1{'\n'}
        - Ability to track the figure turns through the round{'\n'}
        - Keep character initiatives hidden until draw{'\n'}
        - Maintain characters between scenarios{'\n'}
        - Use browser cookies to remember previously connected session id.{'\n'}
        - Ability to track Objectives/Escorts/Allies{'\n'}
        - Track Gold and XP for characters{'\n'}
        - Custom counters for characters (for progress on battle goals, etc.){'\n'}
        - Display monster/boss bass stats (hp, attack, move, range){'\n'}
        - Display trap damage, coin value, and base experience gained for scenario{'\n'}
        - Ability to edit base monster stats for scenario rules that modify them (ie. named monsters, demons with extra attack, etc,){'\n'}
        {'\n'}
        v1.2{'\n'}
        - Support for Forgotten Circles{'\n'}
        {'  '}- New scenarios and monsters{'\n'}
        {'  '}- Diviner{'\n'}
        {'  '}- New mechanics{'\n'}
        {'  '}{'  '}- Deck manipulation{'\n'}
        {'  '}{'  '}- Regenerate{'\n'}
        {'\n'}
        v1.3{'\n'}
        - Support for Jaws of the Lion{'\n'}
        {'  '}- New character classes{'\n'}
        {'  '}- New scenarios and monsters{'\n'}
        {'  '}- New mechanics{'\n'}
        {'\n'}
        v1.4{'\n'}
        - Support for Crimson Scales{'\n'}
        {'  '}- New character classes{'\n'}
        {'  '}- New scenarios and monsters{'\n'}
        {'  '}- New mechanics{'\n'}
        {'  '}{'  '}- Multi-wound{'\n'}
        {'\n'}
        v2.0{'\n'}
        - Support for Frosthaven{'\n'}
        {'  '}- New classes{'\n'}
        {'  '}- New scenarios and monsters{'\n'}
        {'  '}- New game mechanics{'\n'}
        - Make UI prettier/more on theme{'\n'}
        {'\n'}
        Unplanned{'\n'}
        - Automatic monster abilities (ie monster summons, ooze split){'\n'}
        - Automatically remove statuses at end of turn/round when appropriate{'\n'}
        - Combine ability defenses with innate defense values (total shield/retaliate, etc.){'\n'}
        - reset at end of round{'\n'}
        - Make available offline somehow (not sure if possible with current infrastruture){'\n'}
        - Better handle different screen sizes{'\n'}
        - Improve joining sessions - maybe use logins and invite friends, etc.{'\n'}
    </Typography>);
}