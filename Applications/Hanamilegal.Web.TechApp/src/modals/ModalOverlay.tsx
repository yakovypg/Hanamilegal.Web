import "styles/modals/modal-overlay.css";

import React, { type JSX } from "react";
import { createPortal } from "react-dom";

interface Props {
  show: boolean;
  content: React.ReactNode;
  closeAfterClickOnBlurredArea: boolean;
  onClose?: () => void;
}

export const ModalOverlay: React.FC<Props> = ({
  show,
  content,
  closeAfterClickOnBlurredArea,
  onClose
}: Props) => {
  if (!show) {
    return null;
  }

  const handleBackdropClick: () => void = () => {
    if (closeAfterClickOnBlurredArea && onClose) {
      onClose();
    }
  };

  const modal: JSX.Element = (
    <div className="modal-dialog" aria-modal="true" role="dialog">
      <div className="blurred-container" onClick={handleBackdropClick} />
      <div
        className="content-container"
        onClick={(e: React.MouseEvent<HTMLDivElement, MouseEvent>) => e.stopPropagation()}>
        {content}
      </div>
    </div>
  );

  return createPortal(modal, document.body);
};
