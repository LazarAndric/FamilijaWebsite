import React, { Component } from "react";

class Log extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isAuthenticated: false,
      resData: "",
    };
  }
  componentDidMount() {
    const payload = {
      username: "ljubica.zivancevic@gmail.com",
      password: "password",
    };
    const requestOptions = {
      method: "POST",
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(payload),
    };
    fetch(
      "http://flixteam.myqnapcloud.com:49300/Authorizations/logIn",
      requestOptions
    )
      .then((res) => res.json())
      .then((data) => {
        console.log(data);
      });
  }
  render() {
    return <div>aaaaaaaaaa :{this.state.resData}</div>;
  }
}
export default Log;
