import React from "react";
import { makeStyles, useTheme } from "@material-ui/core/styles";
import Card from "@material-ui/core/Card";
import CardContent from "@material-ui/core/CardContent";
import CardMedia from "@material-ui/core/CardMedia";
import IconButton from "@material-ui/core/IconButton";
import Typography from "@material-ui/core/Typography";
import SkipPreviousIcon from "@material-ui/icons/SkipPrevious";
import PlayArrowIcon from "@material-ui/icons/PlayArrow";
import SkipNextIcon from "@material-ui/icons/SkipNext";
import Btn from "../Buttons/Button";
import Hidden from '@material-ui/core/Hidden';

const useStyles = makeStyles((theme) => ({
  root: {
    display: "flex",
    justifyContent:"center",
    backgroundColor: "transparent",
    border: "none",
    boxShadow: "none",
  },
  details: {
    display: "flex",
    flexDirection: "column",
  },
  content: {
    flex: "1 0 auto",
  },
  image: {
    width: "100%",
    height: "auto",
  },
  cardContainer:{
    width: "100%",
    margin : "auto"

  },
leftSide:{
  width : "20%"

},
rightSide:{
  width: '80%'
}
}));

export default function CardLeft(props) {
  const classes = useStyles();
  const theme = useTheme();
  const visibility = props.visibility;
  const clas = classes.visibility;


 
  return (
    <div className={classes.cardContainer}>
<Card className={classes.root}>
  <div className={classes.leftSide}>
  <CardMedia src={props.src} title="Live from space album cover" />
      <img src={props.src} className={classes.image}></img>
  </div>

      <div className={classes.details, classes.rightSide}>
        <CardContent className={classes.content}>
          <Typography component="h5" variant="h5">
            {props.title}
          </Typography>
          <Typography variant="subtitle3" color="textSecondary">
            {props.text}
          </Typography>
        </CardContent>
 
       
      </div>
     
    </Card>
    </div>
    
  );
}
