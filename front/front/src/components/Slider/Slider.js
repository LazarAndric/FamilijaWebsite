import React from 'react';

const Slider = () => {
    return (
<div id="carouselExampleControls" class="carousel slide" data-bs-ride="carousel">
  <div class="carousel-inner">
    <div class="carousel-item active">
      <img src="https://www.geographicexperiences.com/wp-content/uploads/revslider/home5/placeholder-1200x500.png" class="d-block w-100" alt="..."/>
    </div>
    <div class="carousel-item">
      <img src="https://www.geographicexperiences.com/wp-content/uploads/revslider/home5/placeholder-1200x500.png" class="d-block w-100" alt="..."/>
    </div>
    <div class="carousel-item">
      <img src="https://www.geographicexperiences.com/wp-content/uploads/revslider/home5/placeholder-1200x500.png" class="d-block w-100" alt="..."/>
    </div>
  </div>
  <a class="carousel-control-prev" href="#carouselExampleControls" role="button" data-bs-slide="prev">
    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
    <span class="visually-hidden">Previous</span>
  </a>
  <a class="carousel-control-next" href="#carouselExampleControls" role="button" data-bs-slide="next">
    <span class="carousel-control-next-icon" aria-hidden="true"></span>
    <span class="visually-hidden">Next</span>
  </a>
</div>
    );
};

export default Slider;