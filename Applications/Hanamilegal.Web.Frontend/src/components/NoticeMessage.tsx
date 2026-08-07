import "styles/components/notice-message.css";

import { type RefObject, useLayoutEffect, useRef, useState } from "react";
import type { Notice } from "types";

interface Props {
  readonly className?: string;
  readonly notice: Notice | null;
  readonly visibleMs?: number;
  readonly hideAfterMs?: number;
}

export const NoticeMessage: React.FC<Props> = ({
  className,
  notice,
  visibleMs = 2000,
  hideAfterMs = 1000
}: Props) => {
  const [isFadingOut, setIsFadingOut] = useState<boolean>(false);
  const [isRendered, setIsRendered] = useState<boolean>(false);

  const fadeOutTimeoutIdRef: RefObject<number | null> = useRef<number | null>(null);
  const removeTimeoutIdRef: RefObject<number | null> = useRef<number | null>(null);

  useLayoutEffect(() => {
    if (notice) {
      if (fadeOutTimeoutIdRef.current) {
        window.clearTimeout(fadeOutTimeoutIdRef.current);
      }

      if (removeTimeoutIdRef.current) {
        window.clearTimeout(removeTimeoutIdRef.current);
      }

      const rafId: number = requestAnimationFrame(() => {
        setIsRendered(true);
        setIsFadingOut(false);
      });

      fadeOutTimeoutIdRef.current = window.setTimeout(() => {
        setIsFadingOut(true);

        removeTimeoutIdRef.current = window.setTimeout(() => {
          setIsRendered(false);
          setIsFadingOut(false);
        }, hideAfterMs);
      }, visibleMs);

      return () => {
        cancelAnimationFrame(rafId);
      };
    } else {
      if (fadeOutTimeoutIdRef.current) {
        window.clearTimeout(fadeOutTimeoutIdRef.current);
      }

      if (removeTimeoutIdRef.current) {
        window.clearTimeout(removeTimeoutIdRef.current);
      }

      const rafId: number = requestAnimationFrame(() => {
        setIsRendered(false);
        setIsFadingOut(false);
      });

      return () => {
        cancelAnimationFrame(rafId);
      };
    }
  }, [notice, visibleMs, hideAfterMs]);

  if (!isRendered || !notice) {
    return null;
  }

  const colorClass: string = notice.isError ? "color-error" : "color-success";
  const hidingClass: string = isFadingOut ? "notice-message--hiding" : "";

  return (
    <div className={`mt-2 normal-text notice-message ${className} ${colorClass} ${hidingClass}`}>
      {notice.message}
    </div>
  );
};
