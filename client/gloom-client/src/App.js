import './App.css';
import {
    AppBar,
    Box,
    Container, createTheme, Dialog, DialogContent, DialogTitle,
    Drawer,
    FormControl,
    IconButton,
    Paper,
    TextField, ThemeProvider,
    Toolbar,
    Typography
} from "@mui/material";
import useScrollTrigger from '@mui/material/useScrollTrigger';
import PropTypes from 'prop-types';
import React, {useEffect, useState} from 'react';
import MenuIcon from '@mui/icons-material/Menu';
import Button from "@mui/material/Button";
import Settings from "./Settings";
import Scenario from "./Scenario";
import {Update} from "@mui/icons-material";

const theme = createTheme({
    palette: {
        primary: {
            main: '#0971f1',
            darker: '#053e85',
        },
        fire: {
            main: "#d81b60",
            light: "#ff5c8d",
            dark: "#a00037"
        },
        ice: {
            main: "#4fc3f7",
            light: "#8bf6ff",
            dark: "#0093c4"
        },
        earth: {
            main: "#43a047",
            light: "#76d275",
            dark: "#00701a"
        },
        air: {
            main: "#757575",
            light: "#a4a4a4",
            dark: "#494949"
        },
        light: {
            main: "#fdd835",
            light: "#ffff6b",
            dark: "#c6a700"
        },
        dark: {
            main: "#212121",
            light: "#484848",
            dark: "#000",
            contrastText: "#bdbdbd"
        },
        bar: {
            main: "#795548",
            light: "#a98274",
            dark: "#4b2c20",
            contrastText: "#bdbdbd"
        },
        elite: {
            main: "#fbc02d",
            light: "#fff263",
            dark: "#c49000"
        }
    },
});

function App(props) {
    const [settingsOpen, setSettingsOpen] = useState(false);
    const [session, setSession] = useState({"Id": -1});
    const [scenario, setScenario] = useState({});
    const [characters, setCharacters] = useState([]);
    const [isScenarioLoaded, setScenarioLoaded] = useState(false);
    const [joinOpen, setJoinOpen] = useState(false);
    const [joiningId, setJoiningId] = useState('0');
    
    useEffect(() => {
       //setInterval(reloadScenario, 2000) 
    });

    // async function reloadScenario() {
    //     if (session.Id > -1) {
    //         await getScenarioApi(session.Id);
    //     }
    // }
    
    const setScenarioClicked = (num, level) => {
        const body = JSON.stringify(
            {
                "SessionId": session.Id,
                "Number": parseInt(num),
                "Level": parseInt(level)
            }
        );
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
    
    const updateCharState = (newChar) => {
        const copy = characters;
        const index = copy.findIndex(ch => ch.Name === newChar.Name);
        copy[index] = newChar;
        setCharacters(copy);
    }
    
    const newSessionClick = () => {
        let init = {
            method: 'POST',
            headers: {
                'Content-Type': 'text/plain'
            },
            body: ""

        };
        fetch(
            `http://127.0.0.1:3000/newsession`, init)
            .then(r => r.json())
            .then(json => {
                setSession({"Id": json.SessionId});
            });
    }
    
    const getScenarioApi = (sessId) => {
        const body = JSON.stringify(
            {
                "SessionId": sessId
            }
        );
        let init = {
            method: 'POST',
            headers: {
                'Content-Type': 'text/plain'
            },
            body: body

        };
        fetch(
            `http://127.0.0.1:3000/getscenario`,
            init)
            .then(r => r.json())
            .then(json => {
                setScenario(json);
                setScenarioLoaded(true);
            });
    }
    
    return (
        <div className="App" > 
            {/*style={{backgroundColor: "#bcaaa4"}}*/}
            <ThemeProvider theme={theme}>
                <ElevationScroll {...props}>
                    <AppBar color={"bar"}>
                        <Toolbar>
                            <IconButton
                                size="large"
                                edge="start"
                                color="inherit"
                                aria-label="menu"
                                sx={{mr: 2}}
                                onClick={() => setSettingsOpen(!settingsOpen)}
                                disabled={session.Id === -1}
                            >
                                <MenuIcon/>
                            </IconButton>
                            <Typography variant="h6" component="div">
                                Gloom App
                            </Typography>
                        </Toolbar>
                    </AppBar>
                </ElevationScroll>
              <Settings setScenarioClicked={setScenarioClicked} 
                        open={settingsOpen} setOpen={setSettingsOpen}
                        addCharacter={(c) => setCharacters([...characters, c])}
                        characters={characters}
              />
                <Box sx={{mt: 8, mx:0}}>
                    {
                        session.Id === -1 ? <div>
                            <Button onClick={newSessionClick}>Start New Session</Button>
                            - OR - 
                            <Button onClick={() => setJoinOpen(true)} >Join Session</Button>   
                            <Dialog open={joinOpen} onClose={() => 
                            {
                                setJoinOpen(false); 
                                setSession({"Id": parseInt(joiningId)});
                                getScenarioApi(joiningId);
                            }
                            }>
                                <DialogTitle>Session Id to Join</DialogTitle>
                                <DialogContent>
                                    <DialogContent>
                                        <TextField margin="normal" 
                                                   inputProps={{inputMode: 'numeric'}} 
                                                   label="SESSION ID" 
                                                   type="number"
                                                   onChange={(e) => (setJoiningId(e.target.value))}
                                        />
                                    </DialogContent>
                                </DialogContent>
                            </Dialog>
                        </div> 
                            :
                            <Typography>
                                Session Id: {session.Id}
                            </Typography>
                    }
                    
                    {isScenarioLoaded ? 
                        <Scenario
                            scenario={scenario} setScenarioState={setScenario} characters={characters}
                            sessionId={session.Id}
                            updateCharState={updateCharState}
                        /> : ""}
                </Box>
            </ThemeProvider>
        </div>
    );

}

export default App;

function ElevationScroll(props) {
    const {children} = props;
    const trigger = useScrollTrigger({
        disableHysteresis: true,
        threshold: 0
    });

    return React.cloneElement(children, {
        elevation: trigger ? 4 : 0,
    });
}

ElevationScroll.propTypes = {
    children: PropTypes.element.isRequired,
    /**
     * Injected by the documentation to work in an iframe.
     * You won't need it on your project.
     */
    window: PropTypes.func,
};

// ideas of components to use:
// Drawer for settings
// Stack for list of monsters etc.
// TransitionGroup for transitioning the cards in the stack
//
//
