import "styles/layout/layout.css";

import { Body, Header } from "layout";
import React from "react";

interface Props {
  readonly children: React.ReactNode;
}

export const Layout: React.FC<Props> = ({ children }: Props) => {
  return (
    <div className="d-flex flex-column min-vh-100">
      <Header />
      <Body children={children} />
      <div className="hero-corner-circle" />
    </div>
  );
};
