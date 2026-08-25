import React, { Component } from "react";
import { Navigate } from "react-router-dom";
import { VIEW_PANELS } from "types";
import { getRoutePath } from "utils";

interface State {
  hasError: boolean;
}

interface Props {
  readonly children: React.ReactNode;
}

export class ErrorBoundary extends Component<Props, State> {
  constructor(props: Props) {
    super(props);
    this.state = { hasError: false };
  }

  static getDerivedStateFromError(): State {
    return { hasError: true };
  }

  componentDidCatch(error: Error, errorInfo: React.ErrorInfo): void {
    console.error("Error: ", error, errorInfo);
  }

  render(): React.ReactNode {
    if (this.state.hasError) {
      return <Navigate to={getRoutePath(VIEW_PANELS.error)} replace />;
    }

    return this.props.children;
  }
}
