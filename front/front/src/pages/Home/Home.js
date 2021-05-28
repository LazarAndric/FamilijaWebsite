import React from "react";
//import logo from "./logo.svg";
//import "./App.css";
import Slider from "../../components/Slider/Slider";
import CardLeft from "../../components/CardLeftAlign/CardLeftAlign";
import CardRight from "../../components/CardRightAlign/CardRightAlign";
import Footer from "../../components/Footer/Footer";
import Card from "../../components/Card/Card";
import {
  CssBaseline,
  Grid,
  Container,
  AppBar,
  Toolbar,
  Typography,
  Button,
  spacing,
} from "@material-ui/core";
import { makeStyles } from "@material-ui/core/styles";
import features from "./WhatYouGet.json";

const useStyles = makeStyles((theme) => ({
  container: {
    padding: theme.spacing(0),
    paddingLeft: 0,
    margin: 0,
    overflow: "hidden",
  },
  spaceTopBottom: {
    paddingTop: theme.spacing(4),
    paddingBottom: theme.spacing(2),
  },
  overflowHidden: {
    overflow: "hidden",
  },
  CardItemsContainer: {
    display: "flex",
    justifyContent: "space-between",
    alignContent: "center",
    aliignItems: "center",
    width: "68vw",
    margin: "0 auto",
  },
}));

const Home = () => {
  const classes = useStyles();
  
  return (
    <>
      <CssBaseline />
      <main>
        <div className={classes.container} maxWidth>
          <Typography variant="h2" align="center" color="textPrimary">
            Hertz App
          </Typography>
          <Slider></Slider>
          <div className={classes.overflowHidden}>
            <Grid
              container
              justify="center"
              maxWidth="sm"
              className={classes.spaceTopBottom}
              alignItems="center"
              spacing={2}
            >
              <Grid item className={classes.paddingRight}>
                <Card></Card>
              </Grid>
              <Grid item className={classes.paddingRight}>
                <Card></Card>
              </Grid>
              <Grid item>
                <Card></Card>
              </Grid>
            </Grid>
        
            <div className={classes.CardItemsContainer}>
                  <Grid
                container
                justify="center"
           
                className={classes.spaceTopBottom}
                alignItems="center"
                alignContent="center"
                spacing={6}
              >
                 {features.map((data)=>{
               return <>
               
            <Grid
             item
             justify="center"
             alignItems="center"
             alignContent="center"
             lg={6}
             xs={12}
            
           >
             <CardLeft
               src={data.srcImg}
               title={data.title}
               btnType="primary"
               text={data.text}
               visibility="hidden"
             ></CardLeft>
           </Grid>
        
             
               
               
        
               </>

            
             })}
                
                </Grid></div>
            
         
         
     
             </div>
      
          </div>
   
      </main>
    </>
  );
};

export default Home;
