import Image from 'next/image'

interface MascotDisplayProps {
  src: string
  alt?: string
}

export function MascotDisplay({ src, alt = 'mascot' }: MascotDisplayProps) {
  return (
    <div className="flex justify-center">
      <div className="relative w-[240px] h-[240px] flex-shrink-0">
        <Image
          src={src}
          alt={alt}
          fill
          className="rounded-2xl object-contain"
          unoptimized
          priority
        />
      </div>
    </div>
  )
}
