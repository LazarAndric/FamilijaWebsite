import React from 'react';


const Button = props => {
    return (
<div class="">
  <button class={props.type}>{props.text}</button>

</div>
    );
};
export default Button;