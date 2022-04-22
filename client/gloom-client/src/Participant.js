import React, {useState, useEffect} from "react";
import MonsterGroup from "./MonsterGroup";
import Boss from "./Boss";

export default function Participant(props) {
    function getComponent() {
        switch (props.group.Type) {
            case 'Monster':
                return <MonsterGroup
                    key={props.group.Name}
                    group={props.group}
                    addMonster={props.addMonster}
                    setScenarioState={props.setScenarioState}
                    scenario={props.scenario}
                />;
            case 'Boss':
                return <Boss
                    key={props.group.Name}
                    boss={props.group}
                    setScenarioState={props.setScenarioState}
                    scenario={props.scenario}
                />;
            // case 'Character':
            //     return <Character />;
        }
    }

    return getComponent();
}
