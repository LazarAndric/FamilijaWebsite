import React from "react";

const Nav = () => {
  return (
    <div class="ui secondary pointing menu">
      <a class="active item" href="https://www.google.com">
        Home
      </a>
      <a class="item">Messages</a>
      <a class="item">Friends</a>
      <div class="right menu">
        <a class="ui item">Logout</a>
      </div>
    </div>
  );
};

export default Nav;
