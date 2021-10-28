import React from "react";
import { ImageWithZoom } from "@ptas/react-public-ui-library";

const CustomPasswordExample = () => {
  return (
    <div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <ImageWithZoom imageUrl='https://miro.medium.com/max/1200/1*otyC6CUn6QqeIDfaPr65bQ.jpeg' />
      </div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <ImageWithZoom
          imageUrl='https://previews.123rf.com/images/peshkov/peshkov1705/peshkov170500023/77676363-abstract-white-wallpaper-with-grey-polygonal-pattern-mock-up.jpg'
          showZoomIcon
        />
      </div>
    </div>
  );
};

export default CustomPasswordExample;
