import React from "react";
import { Link as LinkRouter } from "react-router-dom";
import Toolbar from "@material-ui/core/Toolbar";
import Typography from "@material-ui/core/Typography";
import Button from "@material-ui/core/Button";
import AppBar from "@material-ui/core/AppBar";
import { makeStyles } from "@material-ui/core/styles";
import Link from "@material-ui/core/Link";
import Grid from "@material-ui/core/Grid";
import Container from "@material-ui/core/Container";
import Box from "@material-ui/core/Box";

import CssBaseline from "@material-ui/core/CssBaseline";

function Copyright() {
  return (
    <Typography variant="body2" align="center" style={{ color: "#fff" }}>
      {"Copyright © "}
      <Link color="inherit" href="https://material-ui.com/">
        Your Website
      </Link>{" "}
      {new Date().getFullYear()}
      {"."}
    </Typography>
  );
}

const useStyles = makeStyles((theme) => ({
  "@global": {
    ul: {
      margin: 0,
      padding: 0,
      listStyle: "none",
    },
  },

  appBar: {
    borderTop: `1px solid ${theme.palette.divider}`,
    position: "relative",
    bottom: "0px",
    right: 0,
  },

  toolbar: {
    flexWrap: "wrap",
    justifyContent: "space-between",
    background: "#010606",
    alignItems: "center",
  },

  link: {
    margin: theme.spacing(1, 1.5),
    color: "#988f92",
    fontSize: "12px",
    fontVariant: "normal",
  },
  linkTitle: {
    color: "#fff",
  },

  footer: {
    width: "100vw",
    background: "#161a1d",
    borderTop: `1px solid ${theme.palette.divider}`,
    marginTop: theme.spacing(8),
    paddingTop: theme.spacing(3),
    paddingBottom: theme.spacing(3),
    [theme.breakpoints.up("sm")]: {
      paddingTop: theme.spacing(6),
    },
  },
  footerItem: {
    display: "block",
    margin: theme.spacing(1, 1.5),
  },
  footerIcon: {
    height: "40px",
  },
  hover: {
    backgroundColor: "red",
  },
}));
function Icons() {
  const icons = [
    {
      name: "facebook",
      url:
        "http://cdn.boschtools.com/Boschtools/product-registration/facebook@2x.png",
      link: "https://www.google.me/",
    },
    {
      name: "instagram",
      url:
        "http://cdn.boschtools.com/Boschtools/product-registration/instagram@2x.png",
      link: "https://www.google.me/",
    },
    {
      name: "linkedin",
      url:
        "http://cdn.boschtools.com/Boschtools/product-registration/linkedin@2x.png",
      link: "https://www.google.me/",
    },
    {
      name: "twitter",
      url:
        "http://cdn.boschtools.com/Boschtools/product-registration/twitter@2x.png",
      link: "https://www.google.me/",
    },
  ];
  const classes = useStyles();

  return (
    <>
      <Typography className={classes.linkTitle}>
        Follow us for more information
      </Typography>
      {icons.map((i) => (
        <Link href={i.link}>
          <img src={i.url} className={classes.footerIcon}></img>
        </Link>
      ))}
    </>
  );
}
const Footer = () => {
  const classes = useStyles();
  const footers = [
    {
      title: "Company",
      description: ["Team", "History", "Contact us", "Locations"],
    },
    {
      title: "Features",
      description: [
        "Cool stuff",
        "Random feature",
        "Team feature",
        "Developer stuff",
        "Another one",
      ],
    },
    {
      title: "Resources",
      description: [
        "Resource",
        "Resource name",
        "Another resource",
        "Final resource",
      ],
    },
    {
      title: "Legal",
      description: ["Privacy policy", "Terms of use"],
    },
  ];

  return (
    <>
      <Container
        component="footer"
        className={classes.footer}
        container
        maxWidth="lg"
      >
        <Grid container spacing={4} justify="space-evenly">
          {footers.map((footer) => (
            <Grid
              item
              xs={4}
              sm={2}
              key={footer.title}
              className={classes.footerItem}
            >
              <Typography
                variant="h6"
                color="textSecondary"
                className={classes.linkTitle}
                gutterBottom
              >
                {footer.title}
              </Typography>
              <ul>
                {footer.description.map((item) => (
                  <li key={item}>
                    <Link href="#" variant="subtitle1" className={classes.link}>
                      {item}
                    </Link>
                  </li>
                ))}
              </ul>
            </Grid>
          ))}
        </Grid>

        <Box mt={5} align="center">
          <Icons></Icons>
        </Box>
        <Box mt={5}>
          <Copyright />
        </Box>
      </Container>
    </>
  );
};

export default Footer;
