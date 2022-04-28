import React, {useState} from "react";
import Button from "@mui/material/Button";
import Scenario from "./Scenario";
import {FormControl, TextField} from "@mui/material";

export function PageLayout(props) {
    return (<div>
        {getScenarioComponent()}
    </div>);
    
    function getScenarioComponent() {
        if (props.isScenarioLoaded)
            return (<Scenario scenario={props.scenario} setScenarioState={props.setScenario} reduceElements={props.reduceElements}/>);
    }

}