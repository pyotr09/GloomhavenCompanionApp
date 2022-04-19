import logo from './logo.svg';
import './App.css';
import {PageLayout} from "./PageLayout";
import {Container} from "@mui/material";

function App() {
  return (
    <div className="App">
        <Container maxWidth="lg">
            <PageLayout />
        </Container>
    </div>
  );
}

export default App;
