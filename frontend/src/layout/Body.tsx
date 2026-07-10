import { PageTransition } from "components";
import React from "react";
import { Container } from "react-bootstrap";

interface Props {
  readonly children: React.ReactNode;
}

export const Body: React.FC<Props> = ({ children }: Props) => {
  return (
    <PageTransition className="flex-grow-1">
      <main>
        <Container className="py-4">{children}</Container>
      </main>
    </PageTransition>
  );
};
