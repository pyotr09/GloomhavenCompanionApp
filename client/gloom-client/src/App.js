import './App.css';
import {PageLayout} from "./PageLayout";
import {
    AppBar,
    Box,
    Container, createTheme,
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
import React, {useState} from 'react';
import MenuIcon from '@mui/icons-material/Menu';
import Button from "@mui/material/Button";

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
        }
    },
});

function App(props) {
    const [settingsOpen, setOpen] = useState(false);
    const [sNum, setScenarioNum] = useState('0');
    const [sLevel, setScenarioLevel] = useState('0');
    const [scenario, setScenario] = useState({});
    const [isScenarioLoaded, setScenarioLoaded] = useState(false);

    const [fire, setFire] = useState(0);
    const [ice, setIce] = useState(0);
    const [earth, setEarth] = useState(0);
    const [air, setAir] = useState(0);
    const [light, setLight] = useState(0);
    const [dark, setDark] = useState(0);

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
                                onClick={() => setOpen(!settingsOpen)}
                            >
                                <MenuIcon/>
                            </IconButton>
                            <Typography variant="h6" component="div">
                                Gloom App
                            </Typography>
                        </Toolbar>
                    </AppBar>
                </ElevationScroll>
                <Drawer
                    anchor={'left'}
                    open={settingsOpen}
                    onClose={() => setOpen(false)}
                >
                    <FormControl>
                        <TextField margin="normal" inputProps={{inputMode: 'numeric'}} label="Level" type="number"
                                   onChange={(e) => setScenarioLevel(e.target.value)}/>
                        <TextField margin="normal" inputProps={{inputMode: 'numeric'}} label="Number" type="number"
                                   onChange={(e) => setScenarioNum(e.target.value)}/>
                        <Button onClick={() => setScenarioClicked()}>Set Scenario</Button>
                    </FormControl>
                </Drawer>
                <Container maxWidth={false}>
                    <Box sx={{mt: 8}}>
                        <Button
                            style={getElementButtonStyle(fire, "#d81b60")}
                            variant={getElementButtonVariant(fire)}
                            color={"fire"}
                            onClick={() => {
                                if (fire === 0) setFire(2); else setFire(0);
                            }}
                            onDoubleClick={() => setFire(1)}
                        >
                            FIRE
                        </Button>
                        <Button style={getElementButtonStyle(ice, "#4fc3f7")}
                                variant={getElementButtonVariant(ice)}
                                color={"ice"}
                                onClick={() => {
                                    if (ice === 0) setIce(2); else setIce(0);
                                }}
                                onDoubleClick={() => setIce(1)}
                        >
                            ICE
                        </Button>
                        <Button style={getElementButtonStyle(earth, "#43a047")}
                                variant={getElementButtonVariant(earth)}
                                color={"earth"}
                                onClick={() => {
                                    if (earth === 0) setEarth(2); else setEarth(0);
                                }}
                                onDoubleClick={() => setEarth(1)}
                        >
                            EARTH
                        </Button>
                        <Button style={getElementButtonStyle(air, "#757575")}
                                variant={getElementButtonVariant(air)}
                                color={"air"}
                                onClick={() => {
                                    if (air === 0) setAir(2); else setAir(0);
                                }}
                                onDoubleClick={() => setAir(1)}
                                
                        >
                            Air
                        </Button>
                        <Button style={getElementButtonStyle(light, "#fdd835")}
                                variant={getElementButtonVariant(light)}
                                color={"light"}
                                onClick={() => {
                                    if (light === 0) setLight(2); else setLight(0);
                                }}
                                onDoubleClick={() => setLight(1)}
                        >
                            LIGHT
                        </Button>
                        <Button style={getElementButtonStyle(dark, "#212121")}
                                variant={getElementButtonVariant(dark)}
                                color={"dark"}
                                onClick={() => {
                                    if (dark === 0) setDark(2); else setDark(0);
                                }}
                                onDoubleClick={() => setDark(1)}
                        >
                            DARK
                        </Button>
                        <PageLayout isScenarioLoaded={isScenarioLoaded} scenario={scenario} setScenario={setScenario}
                                    reduceElements={reduceElements}/>
                    </Box>
                </Container>
            </ThemeProvider>
        </div>
    );
    
    function reduceElements() {
        if (fire > 0)
            setFire(fire - 1);
        if (ice > 0)
            setIce(ice - 1);
        if (earth > 0)
            setEarth(earth - 1);
        if (air > 0)
            setAir(air - 1);
        if (light > 0)
            setLight(light - 1);
        if (dark > 0)
            setDark(dark - 1);
    }

    function getElementButtonVariant(el) {
        if (el === 0) {
            return "outlined";
        }
        if (el === 1) {
            return "contained";
        }
        if (el === 2) {
            return "contained";
        }
    }

    function getElementButtonStyle(el, color) {
        if (el === 0) {
            return {};
        }
        if (el === 1) {
            return {
                background: `linear-gradient(to bottom, Transparent 0%,Transparent 50%,${color} 50%,${color} 100%)`
            };
        }
        if (el === 2) {
            return {};
        }
    }

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
// Speed dial for adding statuses
// Drawer for settings
// Stack for list of monsters etc.
// Popper (instead of Dialog) for monster number adder
// TransitionGroup for transitioning the cards in the stack
//
//
