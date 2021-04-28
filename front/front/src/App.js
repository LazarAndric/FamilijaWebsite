import React from "react";
import {BrowserRouter as Router, Switch, Route} from "react-router-dom"

import Home from "./pages/Home/Home"
import About from "./pages/About/About"
import Iptv from "./pages/IPTV/Iptv"
import Nav from "./components/Nav/Nav"
import Sidebar from "./components/Sidebar/Sidebar"


const App = () => {
  return (
    <>
      <Router>
        <Nav/>
        <Switch>
        <Route path="/" exact component={Home}/>
        <Route path="/about" component={About}/>
        <Route path="/iptv" component={Iptv}/>
        </Switch>
      </Router>
    </>
  );
};

export default App;
