import { HeaderDesktop, HeaderMobile } from "layout";
import React, { useEffect, useState } from "react";
import { DESKTOP_SCREEN_MIN_WIDTH } from "utils";

const useWindowSize = () => {
  const [windowSize, setWindowSize] = useState({
    width: window.innerWidth,
    height: window.innerHeight
  });

  useEffect(() => {
    const handleResize = () => {
      setWindowSize({
        width: window.innerWidth,
        height: window.innerHeight
      });
    };

    window.addEventListener("resize", handleResize);

    return () => {
      window.removeEventListener("resize", handleResize);
    };
  }, []);

  return windowSize;
};

export const Header: React.FC = () => {
  const { width } = useWindowSize();

  return width < DESKTOP_SCREEN_MIN_WIDTH ? <HeaderMobile /> : <HeaderDesktop />;
};
