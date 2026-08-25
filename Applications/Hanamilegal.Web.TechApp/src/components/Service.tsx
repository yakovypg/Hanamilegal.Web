import "styles/components/service.css";

import React from "react";

interface Props {
  readonly name: string;
  readonly icon: React.ReactNode;
  readonly className?: string;
  readonly onClick?: React.MouseEventHandler<HTMLDivElement>;
}

export const Service: React.FC<Props> = ({ name, icon, className, onClick }: Props) => {
  return (
    <div className={`service-box ${className}`} onClick={onClick}>
      <p className="mb-2 service-icon-name">{name}</p>
      <div className="service-icon-box">{icon}</div>
    </div>
  );
};
