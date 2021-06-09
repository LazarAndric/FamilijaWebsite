import React, {useState, useEffect} from "react";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import Cookies from 'universal-cookie';

import Home from "./pages/Home/Home";
import Contact from "./pages/Contact/Contact";
import Iptv from "./pages/IPTV/Iptv";
import Nav from "./components/Nav/Nav";
import Sidebar from "./components/Sidebar/Sidebar";
import Footer from "./components/Footer/Footer";
import Login from "./pages/Login/Login";
import Log from "./pages/Login/Log";
import Register from "./pages/Register/Register";

const App = () => {
  const cookie= new Cookies()
  const jwt = cookie.get('jwt')
  const refresh = cookie.get('refresh')
  return (
    <>
      <Router>
        <Nav isLogin={!jwt || !refresh}/>
        <Switch>
          <Route path="/" exact component={Home} />
          <Route path="/Contact" component={Contact} />
          <Route path="/iptv" component={Log} />
          <Route path="/login" component={Login} />
          <Route path="/register" component={Register} />
        </Switch>
        <Footer />
      </Router>
    </>
  );
};

export default App;
