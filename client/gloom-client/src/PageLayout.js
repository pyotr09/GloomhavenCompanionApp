import React, {useState} from "react";
import Button from "@mui/material/Button";
import Scenario from "./Scenario";
import {TextField} from "@mui/material";

export function PageLayout() {
    const [sNum, setScenarioNum] = useState('0');
    const [sLevel, setScenarioLevel] = useState('0');
    const [scenario, setScenario] = useState({});
    const [isScenarioLoaded, setScenarioLoaded] = useState(false);
    
    const setScenarioClicked = () => {
        const body = JSON.stringify(
            {
                "Number": parseInt(sNum),
                "Level": parseInt(sLevel)
            }
        );
        console.log(body);
        let init = {
            method: 'POST',
            headers: {
                'Content-Type': 'text/plain'
            },
            body: body

        };
        fetch(
            `http://127.0.0.1:3000/setscenario`,
            init)
            .then(r => r.json())
            .then(json => {
                setScenario(json);
                setScenarioLoaded(true);
            });
    }
    
    return (<div>
        <TextField label="Level" type="number" onChange={(e) => setScenarioLevel(e.target.value)}/>
        <TextField label="Number" type="number" onChange={(e) => setScenarioNum(e.target.value)}/>
        <Button onClick={() => setScenarioClicked()}>Set Scenario</Button>
        {getScenarioComponent()}
    </div>);
    
    function getScenarioComponent() {
        if (isScenarioLoaded)
            return (<Scenario scenario={scenario} setScenarioState={setScenario} />);
    }

}