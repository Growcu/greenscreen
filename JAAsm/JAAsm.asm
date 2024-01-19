
.data
	colorToRemove db 16 dup (?) ;Array which will fill xmm register with bytest from color to remove.
.code
removeGreenScreenASM PROC
		;Move data aqquired from C# function call to general purpose registers.
		MOV r9, rcx ;Photo pixels in bytes.
		MOV r10, rdx ;Removable color.
		MOV r11, r8 ;Amount of bytes (in array stored in r9 register).

		XOR rcx, rcx ;Clear counter.
		MOV rax, OFFSET colorToRemove ;Move array address ro RAX 

;Fill colorToRemove with color to remove.
fillXmm:
		;Alpha is hardcoded to 255
		XOR r12,r12
		NOT r12
		MOV [rax], r12b
		
		;Red
		ADD rax,TYPE colorToRemove 
		MOV r12b, [r10]
		MOV [rax], r12b
		
		;Green
		ADD rax,TYPE colorToRemove 
		MOV r12b, [r10+1]
		MOV [rax], r12b

		;Blue
		ADD rax,TYPE colorToRemove 
		MOV r12b, [r10+2]
		MOV [rax], r12b

		;Move further in colorToRemove and increment counter by 4.
		ADD rax,TYPE colorToRemove 
		ADD rcx, 4

		;Chcek if whole array was filled.
		CMP rcx, 16
		JNE fillXmm
		
		;Use array to fill xmm0 register
		MOV rax, OFFSET colorToRemove
		MOVDQU xmm0, [rax] ;Move double quad word unalligned

;Procces picture data 
		MOV rax, r9 ;Push pixelArray (in bytes) to RAX
sseLoop:		
		MOVDQU xmm1, [rax] ;Move 16 bytes (4 pixels) from PixelArray to xmm
		PCMPEQD xmm0, xmm1 ;Check if quadwords (pixels) are equals if so change in xmm0 pixel bits to 1
		PCMPEQD xmm2, xmm2 ;Set all xmm2 bits to 1
		PXOR xmm0,xmm2 ;PXOR + previous instruction makes bitwise negation of xmm0 bits 
		PAND xmm1, xmm0 ;Clear appropriate bits in xmm1
		MOVDQU [rax], xmm1 ;Save changed pixels back to array

		ADD rax, 16 ;Mov to next bytes in pixelArray
		
		SUB r11, 16 ;Reduce unprocessed bytes amount by 16
		CMP r11, 0 ;Check if bytes amount is enough to fill xmm register.
		JLE restPixelLoop
		
		
		MOV r13, OFFSET colorToRemove 
		MOVDQU xmm0, [r13]
		JMP sseLoop

;Responsible for rest of pixels
restPixelLoop:
		add r11, 16

exit:
mov rax,0
ret

removeGreenScreenASM ENDP

end