import { ModalOverlay } from "modals";
import React, { createContext, useCallback, useContext, useMemo, useState } from "react";

interface OpenOptions {
  content: React.ReactNode;
  closeAfterClickOnBlurredArea: boolean;
}

interface ModalContextType {
  openModal: (opts: OpenOptions) => void;
  closeModal: () => void;
  isOpen: boolean;
}

interface ModalProviderProps {
  children: React.ReactNode;
}

const ModalContext: React.Context<ModalContextType | undefined> = createContext<
  ModalContextType | undefined
>(undefined);

export const ModalProvider: React.FC<ModalProviderProps> = ({ children }: ModalProviderProps) => {
  const [isOpen, setIsOpen] = useState(false);
  const [content, setContent] = useState<React.ReactNode>(null);
  const [closeAfterClickOnBlurredArea, setCloseAfterClickOnBlurredArea] = useState<boolean>(true);

  const openModal: (opts: OpenOptions) => void = useCallback((opts: OpenOptions) => {
    setContent(opts.content);
    setCloseAfterClickOnBlurredArea(opts.closeAfterClickOnBlurredArea);
    setIsOpen(true);
    document.body.style.overflow = "hidden";
  }, []);

  const closeModal: () => void = useCallback(() => {
    setIsOpen(false);
    setTimeout(() => setContent(null), 200);
    document.body.style.overflow = "";
  }, []);

  const value: {
    openModal: (opts: OpenOptions) => void;
    closeModal: () => void;
    isOpen: boolean;
  } = useMemo(() => ({ openModal, closeModal, isOpen }), [openModal, closeModal, isOpen]);

  return (
    <ModalContext.Provider value={value}>
      {children}
      <ModalOverlay
        show={isOpen}
        content={content}
        closeAfterClickOnBlurredArea={closeAfterClickOnBlurredArea}
        onClose={closeModal}
      />
    </ModalContext.Provider>
  );
};

// eslint-disable-next-line react-refresh/only-export-components
export const useModal = (): ModalContextType => {
  const ctx: ModalContextType | undefined = useContext(ModalContext);

  if (!ctx) {
    throw new Error("useModal must be used within ModalProvider");
  }

  return ctx;
};
