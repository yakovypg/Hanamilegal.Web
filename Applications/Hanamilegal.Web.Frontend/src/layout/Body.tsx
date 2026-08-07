import { PageTransition } from "components";
import React from "react";

interface Props {
  readonly children: React.ReactNode;
}

export const Body: React.FC<Props> = ({ children }: Props) => {
  return (
    <PageTransition className="flex-grow-1">
      <main>
        <div>{children}</div>
      </main>
    </PageTransition>
  );
};
