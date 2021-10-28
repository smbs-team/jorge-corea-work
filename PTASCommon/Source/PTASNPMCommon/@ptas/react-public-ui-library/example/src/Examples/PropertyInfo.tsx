import React from "react";
import { PropertyInfo } from "@ptas/react-public-ui-library";

const CustomPasswordExample = () => {
  return (
    <div>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        <PropertyInfo
          urlImages={[
            "https://miro.medium.com/max/1200/1*otyC6CUn6QqeIDfaPr65bQ.jpeg",
            "https://venngage-wordpress.s3.amazonaws.com/uploads/2018/09/Colorful-Circle-Simple-Background-Image-1.jpg",
            "https://i.ytimg.com/vi/DHcnRcCPM0M/maxresdefault.jpg",
            "https://static1.makeuseofimages.com/wordpress/wp-content/uploads/2017/02/Photoshop-Replace-Background-Featured.jpg"
          ]}
        >
          <span style={{ color: "black" }}>
            Lorem ipsum dolor sit amet consectetur, adipisicing elit. Error eos
            animi laudantium quos, iusto voluptas quasi nemo velit laboriosam
            repellat in reprehenderit assumenda accusantium aliquam at vel
            pariatur exercitationem eius!
          </span>
        </PropertyInfo>
      </div>
    </div>
  );
};

export default CustomPasswordExample;
