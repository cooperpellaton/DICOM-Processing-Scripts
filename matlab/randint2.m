function randint2 = randint2(outputRow,outputCol,outputRange,varargin)

% RANDINT2(m,n,range) is an M-by-N matrix with random integer entries drawn with replacement from
% elements of vector RANGE. The elements in vector RANGE do not need to be contiguous or unique.
%
% To specify a contiguous integer range from Xlow to Xhigh, use RANGE = [Xlow:Xhigh].
%
% RANDINT2(m,n,range,'noreplace') is an M-by-N matrix with random integers drawn without replacement

if isequal(size(outputRange),[1,2]) && ~isequal(outputRange(1),outputRange(2)-1),
    warning('To specify a range [low high] use [low:high].')
end

if ~isequal(round(outputRange),outputRange),
    warning('specified RANGE contains noninteger values.')
end

if ~isequal(length(outputRange),length(outputRange(:))),
    error('Range must be a vector of integer values.')
end

numElements = outputRow*outputCol;

if isempty(varargin),
    
    randint2 = zeros(outputRow,outputCol);
    randIx = floor((length(outputRange))*rand(size(randint2))) + 1;
    randint2 = outputRange(randIx);
    if ~isequal(size(randIx),size(randint2)),
        randint2 = reshape(randint2,size(randIx));
    end
    
elseif isequal(varargin{1},'noreplace'),
    
    if numElements > length(outputRange),
        error('Not enough elements in range to sample without replacement.')
    else
        % Generate full range of integers
        XfullShuffle = outputRange(randperm(length(outputRange)));
        % Select the first bunch
        randint2 = reshape(XfullShuffle(1:numElements),outputRow,outputCol);
    end
    
else
    error('Valid argument is ''noreplace''.')
end
